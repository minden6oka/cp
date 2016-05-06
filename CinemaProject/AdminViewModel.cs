using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaProject
{
    class AdminViewModel : INotifyPropertyChanged
    {
        #region MovieCreateData

        private string title;
        private Visibility mcreateVisibility;
        private string imageUri;
        private string description;
        private int length;
        private DateTime selectedDate;
        private ObservableCollection<DateTime> dates;
        private int hour;
        private int minute;
        private int[] hours;
        private int[] minutes;
        
        public int[] numbers(int n)
        {
            int[] temp = new int[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = i;
            }
            return temp;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ImageUri
        {
            get { return imageUri; }
            set { imageUri = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged();}
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }

        public ObservableCollection<DateTime> Dates
        {
            get { return dates; }
            set { dates = value; }
        }

        public Visibility McreateVisibility
        {
            get { return mcreateVisibility; }
            set { mcreateVisibility = value; OnPropertyChanged();}
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        public int Minute
        {
            get { return minute; }
            set { minute = value; }
        }

        public int[] Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        public int[] Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

        #endregion

        public AdminViewModel()
        {
            dates = new ObservableCollection<DateTime>();
            SelectedDate = DateTime.Today;
            hours = numbers(24);
            minutes = numbers(60);
            mcreateVisibility = Visibility.Collapsed;
        }
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
