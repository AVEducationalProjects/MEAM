using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MEAM.Model;
using Microsoft.Win32;

namespace MEAM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MaintenanceTask> _maintenanceTasks;
        private MaintenancePlan _model;
        private int _timeToRun;

        public MainWindow()
        {
            InitializeComponent();

            _timeToRun = int.Parse(ConfigurationManager.AppSettings["timeToRun"]);
            calcProgress.Maximum = _timeToRun;

            var maintenanceTasks = ConfigurationManager.AppSettings["maintenanceTasks"];
            if(string.IsNullOrWhiteSpace(maintenanceTasks) || !File.Exists(maintenanceTasks))
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Открыть файл задач обслуживания";
                if (openFileDialog.ShowDialog() == true)
                    maintenanceTasks = openFileDialog.FileName;
            }
            _maintenanceTasks = ModelSerializer.LoadTasks(maintenanceTasks);

            var defaultPlan = ConfigurationManager.AppSettings["defaultPlan"];
            if (string.IsNullOrWhiteSpace(defaultPlan) || !File.Exists(defaultPlan))
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Открыть файл плана обслуживания";
                if (openFileDialog.ShowDialog() == true)
                    defaultPlan = openFileDialog.FileName;
            }
            
            _model = ModelSerializer.LoadMaintenancePlan(defaultPlan);

            PlanWindow.DataContext = _model;
        }

        private void CalculatePlan(object sender, RoutedEventArgs e)
        {
            var options = new HiveOptionsDialog();
            if(options.ShowDialog() !=true)
                return;

            var hive = new Hive(_model, _maintenanceTasks, options.Scout, options.Reduce, options.Exchange);
            hive.Run();
            var timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};

            var currentTime = 0;
            timer.Tick += (o, args) =>
            {
                calcProgress.Value = currentTime;
                currentTime++;
                if (currentTime > _timeToRun)
                {
                    timer.Stop();
                    hive.Stop();

                    _model = hive.GetBest();
                    if(_model != null)
                        PlanWindow.DataContext = _model;

                    pProgress.Visibility = Visibility.Hidden;
                    pCalculatons.Visibility = Visibility.Visible;

                    MessageBox.Show($"Было рассмотрено {hive.Count} вариантов.");
                }
            };

            pProgress.Visibility = Visibility.Visible;
            pCalculatons.Visibility = Visibility.Hidden;
            calcProgress.Value = 0;
            timer.Start();
        }

        private void EditObjectParams(object sender, RoutedEventArgs e)
        {

            var ctrl = (Control) sender;
            var objectMaintenancePlan = ctrl.DataContext as ObjectMaintenancePlan;
            if (objectMaintenancePlan != null)
            {
                var objPropertiesDialog = new ObjectPropertiesDialog();
                objPropertiesDialog.DataContext = objectMaintenancePlan.Object;
                objPropertiesDialog.ShowDialog();
                objectMaintenancePlan.CalculateDays();
                var row = (FrameworkElement) ((FrameworkElement) ctrl.Parent).Parent;
                row.DataContext = null;
                row.DataContext = objectMaintenancePlan;

                txtRevenue.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                txtRiskRestriction.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                txtHRRestriction.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                txtCIRestriction.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
                txtTimeRestriction.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                ScaleSlider.Value += (e.Delta > 0) ? 0.1 : -0.1;
            }

        }

        private void LoadPlan(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Открыть файл плана обслуживания";
            if (openFileDialog.ShowDialog() != true)
                return;

            _model = ModelSerializer.LoadMaintenancePlan(openFileDialog.FileName);
            PlanWindow.DataContext = _model;
        }

        private void SavePlan(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog {Title = "Сохранить файл плана обслуживания"};
            if (saveFileDialog.ShowDialog() != true)
                return;

            ModelSerializer.SaveMaintenancePlan(_model, saveFileDialog.FileName);
        }
    }
}
