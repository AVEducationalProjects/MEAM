using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MEAM
{
    /// <summary>
    /// Interaction logic for HiveOptionsDialog.xaml
    /// </summary>
    public partial class HiveOptionsDialog : Window
    {
        public HiveOptionsDialog()
        {
            Scout = 2;
            Reduce = 1;
            Exchange = 1;

            InitializeComponent();
            DataContext = this;
        }

        public int Scout { get; set; }
        public int Reduce { get; set; }
        public int Exchange { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
