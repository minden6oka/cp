using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using CinemaProject.CinemaCervises;

namespace CinemaProject
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private LoginViewModel VM;
        private CancellationTokenSource cts;
        public LoginPage(ref CancellationTokenSource cts)
        {
            InitializeComponent();
            this.cts = cts;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User temp = null;
            try
            {
                temp = Server.ValidateUser(VM.Email, pw.Password.MD5Hash());
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show("Server is offline");
                new Server();
                return;
            }
            switch (temp.UserType1)
            {
                case UserType.AverageUser:
                    case UserType.Moderator://moderator will simply see some extra button in comments and no reservation
                    NavigationService.Navigate(new UserPage(ref cts, temp));
                    break;
                case UserType.Administrator://he will have a unique page
                    NavigationService.Navigate(new AdminPage());
                    break;
                default:
                    MessageBox.Show("Email/Password error");
                    break;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage(ref cts));
        }

        private void Email_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (VM.Email == null || VM.Email == "" || VM.Email.IsEmail())
            {
                VM.SetWarning(String.Empty, LoginWarningType.Email);
            }
            else
            {
                VM.SetWarning("Bad email format: example@example.example", LoginWarningType.Email);
            }
        }

        private void LoginPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            VM = new LoginViewModel();
            DataContext = VM;
        }

        private void Pw_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (pw.Password.Length < 4)
            {
                VM.SetWarning("Pasword is at least 4 characters", LoginWarningType.Password);
            }
            else
            {
                VM.SetWarning(String.Empty, LoginWarningType.Password);
            }
        }
    }
}
