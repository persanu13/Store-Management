using Magazin2.Core;
using Magazin2.Models;
using Magazin2.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Magazin2.ViewModels
{
    public class AdminPageVM : Core.ObservableObject
    {
        private object _currentAdminView;
        public object CurrentAdminView
        {
            get { return _currentAdminView; }
            set { _currentAdminView = value; OnPropertyChanged(); }
        }
        public AdminPageVM()
        {
            CurrentAdminView = new UsersVM();
            RadioButton radBtn = new RadioButton();
            radBtn.Content = "USERS";
            MenuButton(radBtn);
        }

        public RelayCommand MenuButtonCommand => new RelayCommand(MenuButton);
        private void MenuButton(object parameter)
        {
            string menuItem = (parameter as RadioButton).Content.ToString();
            switch (menuItem)
            {
                case "USERS":
                    CurrentAdminView = new UsersVM();
                    break;
                case "PRODUCTS":
                    CurrentAdminView = new ProductsVM();
                    break;
                case "CATEGORIES":
                    CurrentAdminView = new CategoriesVM();
                    break;
                case "MANUFACTURERS":
                    CurrentAdminView = new ManufacturersVM();
                    break;
                case "STOCKS":
                    CurrentAdminView = new StocksVM();
                    break;
                case "RECEIPTS":
                    CurrentAdminView = new ReceiptsVM();
                    break;
                default:
                    MessageBox.Show("Menu Eror!");
                    break;
            }
            
        }

    }
}
