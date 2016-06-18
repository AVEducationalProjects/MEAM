using System;
using System.Collections.Concurrent;
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
        private MaintenancePlan _model;
        private int _timeToRun;

        public MainWindow()
        {
            InitializeComponent();

            _timeToRun = int.Parse(ConfigurationManager.AppSettings["timeToRun"]);
            calcProgress.Maximum = _timeToRun;

            var defaultPlan = ConfigurationManager.AppSettings["defaultPlan"];
            if (string.IsNullOrWhiteSpace(defaultPlan) || !File.Exists(defaultPlan))
            {
                var openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                    defaultPlan = openFileDialog.FileName;
            }

            _model = ModelSerializer.LoadMaintenancePlan(defaultPlan);

            PlanWindow.DataContext = _model;
        }

        private void CalculatePlan(object sender, RoutedEventArgs e)
        {
            var hive = new Hive(_model);
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
    }
}
