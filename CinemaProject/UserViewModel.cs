using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CinemaProject.CinemaCervises;

namespace CinemaProject
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private Movie selectedMovie;
        public ObservableCollection<Movie> Movies { get; set; }
        private Visibility movieListVisibility;
        private Visibility showMovieVisibility;
        private string filterText;
        private User currentUser;
        private SolidColorBrush editColor;
        private bool canEdit;
        private Visibility genderVisibility;
        private Visibility editVisibility;
        private GenderType selectedGender;
        private string editButton;
        private RatingType selectedType;
        private Visibility commentVisibility;
        private string commentText;
        public Array RatingTypos { get { return Enum.GetValues(typeof (RatingType)); } }

        public string GenderSTH 
        {
            get { return currentUser.GenderType1.ToString(); }
        }

        public Array GenderTypes { get { return Enum.GetValues(typeof (GenderType)); } }
        public Movie SelectedMovie
        {
            get { return selectedMovie; }
            set
            {
                selectedMovie = value;
                OnPropertyChanged();
            }
        }

        public Visibility MovieListVisibility
        {
            get { return movieListVisibility; }
            set
            {
                movieListVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowMovieVisibility
        {
            get { return showMovieVisibility; }
            set { showMovieVisibility = value; OnPropertyChanged();}
        }

        public string FilterText
        {
            get { return filterText; }
            set { filterText = value; OnPropertyChanged();}
        }

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged(); }
        }

        public SolidColorBrush EditColor
        {
            get { return editColor; }
            set { editColor = value; OnPropertyChanged(); }
        }

        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; OnPropertyChanged(); }
        }

        public Visibility GenderVisibility
        {
            get { return genderVisibility; }
            set { genderVisibility = value; OnPropertyChanged(); }
        }

        public Visibility EditVisibility
        {
            get { return editVisibility; }
            set { editVisibility = value; OnPropertyChanged(); }
        }

        public GenderType SelectedGender
        {
            get { return selectedGender; }
            set { selectedGender = value; OnPropertyChanged(); }
        }

        public string EditButton
        {
            get { return editButton; }
            set { editButton = value; OnPropertyChanged(); }
        }

        public RatingType SelectedType
        {
            get { return selectedType; }
            set { selectedType = value;  OnPropertyChanged();}
        }

        public Visibility CommentVisibility
        {
            get { return commentVisibility; }
            set { commentVisibility = value; OnPropertyChanged(); }
        }

        public string CommentText
        {
            get { return commentText; }
            set { commentText = value; OnPropertyChanged();}
        }


        public UserViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            MovieListVisibility = Visibility.Visible;
            ShowMovieVisibility = Visibility.Collapsed;
            editButton = "Edit";
            GenderVisibility = Visibility.Visible;
            EditVisibility = Visibility.Collapsed;
            EditColor = Brushes.Black;
            selectedType = RatingType.JustWow;
            CommentVisibility = Visibility.Collapsed;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handle = PropertyChanged;
            if (handle != null)
            {
                handle(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
