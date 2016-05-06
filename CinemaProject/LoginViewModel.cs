using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CinemaProject
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        private string warning;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        public string Warning
        {
            get { return warning; }
            set { warning = value; OnPropertyChanged(); }
        }

        public LoginViewModel()
        {
            warningDic = new Dictionary<LoginWarningType, string>();
            foreach (LoginWarningType args in Enum.GetValues(typeof(LoginWarningType)))
            {
                warningDic.Add(args,String.Empty);
            }
            
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

        private Dictionary<LoginWarningType, string> warningDic; 

        public void SetWarning(string message, LoginWarningType loginWarningType)
        {
            warningDic[loginWarningType] = message;
            StringBuilder sb = new StringBuilder();
            int temp = warningDic.Keys.Count;
            foreach (var args in warningDic)
            {
                temp--;
                if (args.Value != String.Empty)
                {
                    sb.Append(args.Value);
                    if (temp > 0) sb.Append("\n");
                }
            }
            Warning = sb.ToString();
        }
    }

    enum LoginWarningType
    {
        Email,
        Password,
        Authentication
    }
}
