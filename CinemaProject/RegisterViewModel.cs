using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CinemaProject.CinemaCervises;

namespace CinemaProject
{

    class RegisterViewModel : INotifyPropertyChanged
    {
        private string name;
        private string email;
        private string verifyEmail;
        private DateTime birth;
        private GenderType selectedGender;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string VerifyEmail
        {
            get { return verifyEmail; }
            set { verifyEmail = value; }
        }

        public DateTime Birth
        {
            get { return birth; }
            set { birth = value; }
        }

        public GenderType SelectedGender
        {
            get { return selectedGender; }
            set { selectedGender = value; }
        }

        public Array GenderSelection
        {
            get { return Enum.GetValues(typeof (GenderType)); }
        }
        public Dictionary<int, string> Warnings { get; private set; } 
        public RegisterViewModel()
        {
            birth = DateTime.Now.Add(-TimeSpan.FromDays(3650));
            SelectedGender = GenderType.Male;
            Warnings = new Dictionary<int, string>();
            foreach (int args in Enum.GetValues(typeof(RegisterWarningType)))
            {
                Warnings.Add(args, string.Empty);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    enum RegisterWarningType
    {
        Name,
        Email,
        VerifyEmail,
        Password,
        Register
    }
}
