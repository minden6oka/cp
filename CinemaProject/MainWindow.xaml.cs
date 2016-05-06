using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace CinemaProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Server s = new Server();
            cts = new CancellationTokenSource();
            MainFrame.NavigationService.Navigate(new LoginPage(ref cts));
            string[] files = Directory.GetFiles("movies");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            cts.Cancel();
            
        }
    }
}
