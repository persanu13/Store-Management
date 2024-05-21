using Magazin2.Core;
using Magazin2.Models;
using Magazin2.Models.EntityLayer;
using Magazin2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magazin2.ViewModels
{
    class MainWindowVM : Core.ObservableObject
    {
        public LoginVM LoginVm { get; set; }

        private object _currentMainView;
        public object CurrentMainView {
            get{ return _currentMainView; }
            set { _currentMainView = value; OnPropertyChanged(); }
        }

        private Visibility _logoutVsb;
        public Visibility LogoutVsb
        {
            get { return _logoutVsb; }
            set { _logoutVsb = value; OnPropertyChanged(); }
        }

        private Visibility _userVsb;
        public Visibility UserVsb
        {
            get { return _userVsb; }
            set { _userVsb = value; OnPropertyChanged(); }
        }
        private string _userImg;
        public string UserImg
        {
            get { return _userImg; }
            set { _userImg = value; OnPropertyChanged(); }
        }

        public MainWindowVM()
        {
            LoginVm = new LoginVM();
            LogoutVsb = Visibility.Hidden;
            UserVsb = Visibility.Hidden;
            UserImg = "/assets/admin.png";
            CurrentMainView = LoginVm;

            //start with Admin 
            PasswordBox psBox = new PasswordBox();
            psBox.Password = "parola12";
            LoginVm.UserName = "persanu13";
            Login(psBox);
        }
        public RelayCommand LoginCommand => new RelayCommand(Login);
        private void Login(object parameter)
        {
            PasswordBox password = (parameter as PasswordBox);
            Utilizator loggedUser = LoginService.LoginSrv(LoginVm.UserName, password.Password);

            if(loggedUser == null)
            {
                LoginVm.IncorectVsb = Visibility.Visible;
                (parameter as PasswordBox).Password = string.Empty;
                return;
            }

            if (loggedUser.TipUtilizator == "cashier")
            {
                UserImg = "/assets/casier.png";
                LogoutVsb = Visibility.Visible;
                UserVsb = Visibility.Visible;
                CurrentMainView = new CasierPageVM(loggedUser);
                return;
            }

            if (loggedUser.TipUtilizator == "admin")
            {
                UserImg = "/assets/admin.png";
                LogoutVsb = Visibility.Visible;
                UserVsb = Visibility.Visible;
                CurrentMainView = new AdminPageVM();
                return;
            }

        }

        public RelayCommand LogoutCommand => new RelayCommand(Logout);
        private void Logout(object parameter)
        {
            LoginVm.UserName = string.Empty;
            LoginVm.IncorectVsb = Visibility.Hidden;
            CurrentMainView = LoginVm;
            LogoutVsb = Visibility.Hidden;
            UserVsb = Visibility.Hidden;
        }

    }
}
