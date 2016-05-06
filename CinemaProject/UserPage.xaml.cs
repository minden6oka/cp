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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private CancellationTokenSource cts;
        private UserViewModel vm;
        private Movie[] movies;
        private User currentUser;
        public UserPage(ref CancellationTokenSource cts, User currentUser)
        {
            InitializeComponent();
            this.cts = cts;
            this.currentUser = currentUser;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new UserViewModel();
            vm.CurrentUser = currentUser;
            DataContext = vm;
             Dispatcher.Invoke(() =>
             {
                 try
                 {
                     movies = Server.GetMovies();
                     foreach (var movie in movies)
                     {
                         vm.Movies.Add(movie);
                     }
                 }
                 catch (EndpointNotFoundException)
                 {
                     MessageBox.Show("serverOffline");
                 }
                 catch (Exception exc)
                 {
                     MessageBox.Show(exc.Message);
                 }
             });

            
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vm.SelectedMovie != null)
            {
                vm.ShowMovieVisibility = Visibility.Visible;
                vm.MovieListVisibility = Visibility.Collapsed; 
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            vm.ShowMovieVisibility = Visibility.Collapsed;
            vm.MovieListVisibility = Visibility.Visible;
            vm.SelectedMovie = null;
        }

        private Task filter;
        List<Movie> removed = new List<Movie>();
        private string filtering;
        private bool finish;

        private void Filter_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (filter != null && !filter.IsCompleted)
            {
                finish = true;
                filter.Wait();
            }
            filtering = vm.FilterText.ToLower();
            finish = false;
            filter = Task.Run(() =>
            {
                int i = removed.Count - 1;
                while (!finish && i >= 0)
                {
                    if (removed[i].Title.Contains(filtering))
                    {
                        Dispatcher.Invoke(() => vm.Movies.Add(removed[i]));
                        removed.RemoveAt(i);
                    }
                    i--;
                }
                i = 0;
                while (!finish && i < vm.Movies.Count)
                {
                    if (!vm.Movies[i].Title.Contains(filtering))
                    {
                        removed.Add(vm.Movies[i]);
                        Dispatcher.Invoke(() => vm.Movies.RemoveAt(i));
                        continue;
                    }
                    i++;
                }
            },cts.Token);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeElements();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)//needs validation from server
        {
            ChangeElements();
        }

        private void ChangeElements()
        {
            if (vm.EditVisibility == Visibility.Collapsed)
            {
                vm.EditVisibility = Visibility.Visible;
                vm.GenderVisibility = Visibility.Collapsed;
                vm.CanEdit = false;
                vm.EditColor = Brushes.Gray;
            }
            else
            {
                vm.EditVisibility = Visibility.Collapsed;
                vm.GenderVisibility = Visibility.Visible;
                vm.CanEdit = true;
                vm.EditColor = Brushes.Black;
            }
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            vm.SelectedMovie.RatingValue =
                Server.AddRate(new Rating()
                {
                    Title = vm.SelectedMovie.Title,
                    User1 = vm.CurrentUser.Email,
                    RatetyType = vm.SelectedType
                });
            vm.SelectedMovie.RatingType1 = (RatingType) ((int) vm.SelectedMovie.RatingValue);
            vm.OnPropertyChanged("SelectedMovie.RatingType1");
            vm.OnPropertyChanged("vm.SelectedMovie.RatingValue");
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            vm.CommentVisibility = Visibility.Visible;
        }

        private void CommentBack_Click(object sender, RoutedEventArgs e)
        {
            vm.CommentVisibility = Visibility.Collapsed;
        }

        private void CommentClick_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(vm.CommentText);
        }

        private void DeleteComment_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.UserType1 == UserType.AverageUser)
            {

            }
            else if (currentUser.UserType1 == UserType.Moderator)
            {
                
            }
        }
    }
}
