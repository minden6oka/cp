using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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
using CinemaProject.CinemaCervises;

namespace CinemaProject
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private RegisterViewModel Vm;
        private CancellationTokenSource cts;
        public RegisterPage(ref CancellationTokenSource cts)
        {
            InitializeComponent();
            this.cts = cts;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (
                    Server.CreateUser(
                        new User()
                        {
                            Birth = Vm.Birth,
                            Email = Vm.Email,
                            GenderType1 = Vm.SelectedGender,
                            NickName = Vm.Name,
                            Reservations = new Reservation[0],
                            UserType1 = UserType.AverageUser
                        }, pw.Password.MD5Hash()))
                {
                    MessageBox.Show("siker");
                    this.NavigationService.Navigate(new LoginPage(ref cts));
                }
                else
                {
                    MessageBox.Show("Email is already in use");
                }
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Server is offline");
                new Server();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage(ref cts));
        }

        private void RegisterPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            Vm = new RegisterViewModel();
            DataContext = Vm;
        }

        private void Name_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Vm.Name == String.Empty || Vm.Name == null)
            {
                Vm.Warnings[(int) RegisterWarningType.Name] = "This field is mandatory";

            }
            else
            {
                Vm.Warnings[(int)RegisterWarningType.Name] = String.Empty;
            }
            Vm.OnPropertyChanged("Warnings");
        }
    }
}
