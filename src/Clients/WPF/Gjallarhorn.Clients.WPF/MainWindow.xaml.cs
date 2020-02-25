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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gjallarhorn.Client.UWP.ViewModels;
using Gjallarhorn.Clients.WPF.Extenstions;

namespace Gjallarhorn.Clients.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            mainUserControl.Opacity = 0;
            splashScreenUserControl.FadeTo(0, 1000);
            mainUserControl.FadeTo(1, 1000);
        }
    }
}
