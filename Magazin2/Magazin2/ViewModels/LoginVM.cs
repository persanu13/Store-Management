using Magazin2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magazin2.ViewModels
{
    public class LoginVM : Core.ObservableObject
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private Visibility _incorectVsb;
        public Visibility IncorectVsb
        {
            get { return _incorectVsb; }
            set { _incorectVsb = value; OnPropertyChanged(); }
        }

        public LoginVM()
        {
            IncorectVsb = Visibility.Hidden;
        }
       

    }
}
