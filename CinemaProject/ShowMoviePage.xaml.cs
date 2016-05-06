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

namespace CinemaProject
{
    /// <summary>
    /// Interaction logic for ShowMoviePage.xaml
    /// </summary>
    public partial class ShowMoviePage : Page
    {
        private Action changePage;
        public ShowMoviePage(Action changePage = null)
        {
            InitializeComponent();
            this.changePage = changePage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changePage();
            MessageBox.Show("2");
            
        }
    }
}
