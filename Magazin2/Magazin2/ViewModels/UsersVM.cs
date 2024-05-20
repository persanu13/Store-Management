using Magazin2.Core;
using Magazin2.Models;
using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazin2.ViewModels
{
    public class UsersVM : Core.ObservableObject
    {
        private UtilizatorBLL utilizatorBLL = new UtilizatorBLL();
        public ObservableCollection<Utilizator> UtilizatorList {  get; set; }
        private Utilizator updatedUser = null;

        private string _userName = string.Empty;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _userType = string.Empty;
        public bool AdminType
        {
            get { return _userType == "admin"; }
            set { if (value) { _userType = "admin"; }; OnPropertyChanged(); }
        }
        public bool CashierType
        {
            get { return _userType == "cashier"; }
            set { if (value) { _userType = "cashier"; }; OnPropertyChanged(); }
        }
        private string _wrongMessage = string.Empty;
        public string WrongMessage
        {
            get { return _wrongMessage; }
            set { _wrongMessage = value; OnPropertyChanged(); }
        }
        private string _createUpdate = string.Empty;
        public string CreateUpdate
        {
            get { return _createUpdate; }
            set { _createUpdate = value; OnPropertyChanged(); }
        }
        private Visibility _backtoCreate = Visibility.Collapsed;
        public Visibility BacktoCreate
        {
            get { return _backtoCreate; }
            set { _backtoCreate = value; OnPropertyChanged(); }
        }
        private string _searchValue = "";
        public string SearchValue
        {
            get { return _searchValue; }
            set
            { 
                _searchValue = value;
                OnPropertyChanged(); 
                Search(_searchValue, TypeValue);
            }
        }
        private string _type = "0";
        public string TypeIndex
        {
            get { return _type; }
            set 
            { 
                _type = value;
                OnPropertyChanged();
                Search(SearchValue, TypeValue); 
            }
        }
        public string TypeValue
        {
            get {
                if (TypeIndex == "1") return "admin";
                if (TypeIndex == "2") return "cashier";
                return null;
            }
        }

        public UsersVM()
        { 
            UtilizatorList = utilizatorBLL.GetAllUtilizatori();
            CreateUpdate = "Create";
        }

        private void UpdateGuiList(ObservableCollection<Utilizator> newUtilizatorList)
        {
            UtilizatorList.Clear();
            foreach (Utilizator utilizator in newUtilizatorList)
            {
                UtilizatorList.Add(utilizator);
            }
        }

        private bool InputValidation()
        {
            if (UserName.Length < 8)
            {
                WrongMessage = "UserName is too short!";
                return false;
            }
            if (Password.Length < 8)
            {
                WrongMessage = "Password is too short!";
                return false;
            }
            if (_userType == string.Empty)
            {
                WrongMessage = "User type is not selected!";
                return false;
            }
            if (utilizatorBLL.UserNameExist(UserName) && updatedUser?.NumeUtilizator != UserName)
            {
                WrongMessage = "UserName exist!";
                return false;
            }
            return true;
        }
        private void ClearInput()
        {
            UserName = string.Empty;
            Password = string.Empty;
            _userType = string.Empty;
            AdminType = false;
            CashierType = false;
            WrongMessage = string.Empty;
        }

        public RelayCommand AddUpdateUtilizatorCommand => new RelayCommand(AddUpdateUtilizator);
        private void AddUpdateUtilizator(object parameter)
        {
            if (!InputValidation()) return;
            Utilizator newUtilizator = new Utilizator();
            newUtilizator.NumeUtilizator = UserName;
            newUtilizator.Parola = Password;
            newUtilizator.TipUtilizator = _userType;
            if (updatedUser == null)
            {
                utilizatorBLL.AddUtilizator(newUtilizator);
            }
            else
            {
                newUtilizator.UtilizatorID = updatedUser.UtilizatorID;
                utilizatorBLL.UpdateUtilizator(newUtilizator);
                BackToCreate(null);
            }
            ClearInput();
            UpdateGuiList(utilizatorBLL.SearchUtilizatori(SearchValue, TypeValue));
        }

        public RelayCommand DeleteUtilizatorCommand => new RelayCommand(DeleteUtilizator);
        private void DeleteUtilizator(object parameter)
        {
            Utilizator utilizatorDel = (parameter as Utilizator);
            MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to delete {utilizatorDel.NumeUtilizator} ?", "Delete User", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.No) return;
            utilizatorBLL.DeleteUtilizator(utilizatorDel);
            BackToCreate(null);
            UpdateGuiList(utilizatorBLL.SearchUtilizatori(SearchValue,TypeValue));
        }

        public RelayCommand PressUpdateCommand => new RelayCommand(PressUpdate);
        private void PressUpdate(object parameter)
        {
            updatedUser = (parameter as Utilizator);
            UserName = updatedUser.NumeUtilizator;
            Password = updatedUser.Parola;
            if(updatedUser.TipUtilizator == "admin")
            {
                AdminType = true;
            }
            else
            {
                CashierType = true;
            }
            WrongMessage = string.Empty;
            CreateUpdate = "Update";
            BacktoCreate = Visibility.Visible;
        }

        public RelayCommand BackToCreateCommand => new RelayCommand(BackToCreate);
        private void BackToCreate(object parameter)
        {
            CreateUpdate = "Create";
            BacktoCreate = Visibility.Collapsed;
            updatedUser = null;
            ClearInput();
        }
        private void Search(string sir, string tip)
        {
            UpdateGuiList(utilizatorBLL.SearchUtilizatori(sir, tip));
        }


    }

}
