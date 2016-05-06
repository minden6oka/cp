using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
using CinemaProject.CinemaCervises;
using Microsoft.Win32;

namespace CinemaProject
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        AdminViewModel vm;
        public AdminPage()
        {
            InitializeComponent();
        }

        private void CreateMovie_Click(object sender, RoutedEventArgs e)
        {
            if (vm.McreateVisibility == Visibility.Collapsed)
            {
                vm.McreateVisibility = Visibility.Visible;
            }
            else
            {
                vm.McreateVisibility = Visibility.Collapsed;
            }
        }

        private void AdminPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            vm = new AdminViewModel();
            DataContext = vm;
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text[0]);
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            vm.Dates.Add(
                DateTime.Parse(vm.SelectedDate.ToShortDateString() +
                               String.Format("{0:d2}:{1:d2}:00", vm.Hour, vm.Minute)));
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vm.Dates.Remove(
                DateTime.Parse(vm.SelectedDate.ToShortDateString() +
                               String.Format("{0:d2}:{1:d2}:00", vm.Hour, vm.Minute)));
            }
            catch (Exception)
            {
                
            }
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                byte[] image = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap temp = (Bitmap)Bitmap.FromFile(vm.ImageUri);
                    temp.Save(ms,ImageFormat.Jpeg);
                    image = ms.ToArray();
                }
                if (Server.CreateMovie(new Movie()
                {
                    Bmp = image,
                    Description = vm.Description,
                    Length = vm.Length,
                    RatingType1 = RatingType.None,
                    RatingValue = 0,
                    Title = vm.Title,
                    ScreenTimes = vm.Dates.ToArray(),
                    Room = RoomNames.Cheddar
                }))
                {
                    MessageBox.Show("perfect");
                }
                else
                {
                    MessageBox.Show("Already exists");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Server is offline");
                new Server();
            }
            catch (Exception exp)
            {

                MessageBox.Show(exp.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Pictures (.jpg)|*.jpg|All Files (*.*)|*.*";
            if (op.ShowDialog() == true)
            {
                vm.ImageUri = op.FileName;
            }
        }
    }
}
