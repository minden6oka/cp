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
    /// Interaction logic for MovieListPage.xaml
    /// </summary>
    public partial class MovieListPage : Page
    {
        private Action changePage;
        public MovieListPage(Action action = null)
        {
            InitializeComponent();
            changePage = action;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            changePage();
            MessageBox.Show("anyád");
        }
    }
}
