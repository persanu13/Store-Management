using Magazin2.Core;
using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.DataAccessLayer;
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
    public class CategoriesVM : Core.ObservableObject
    {
        private CategorieBLL categorieBLL = new CategorieBLL();
        public ObservableCollection<Categorie> CategorieList { get; set; }

        private Categorie updatedCategori = null;

        private string _categoriName = string.Empty;
        public string CategoriName
        {
            get { return _categoriName; }
            set { _categoriName = value; OnPropertyChanged(); }
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
                UpdateGuiList(categorieBLL.SearchCategori(_searchValue));
            }
        }

        public CategoriesVM() 
        {
            CategorieList = categorieBLL.GetAllCategories();
            CreateUpdate = "Create";
        }

        private void UpdateGuiList(ObservableCollection<Categorie> newCategorieList)
        {
            CategorieList.Clear();
            foreach (Categorie categorie in newCategorieList)
            {
                CategorieList.Add(categorie);
            }
        }

        private bool InputValidation()
        {
            if (string.IsNullOrEmpty(CategoriName))
            {
                WrongMessage = "Categori name is empty!";
                return false;
            }
            if (categorieBLL.CategoryExist(CategoriName))
            {
                WrongMessage = "Categori exist!";
                return false;
            }
            return true;
        }
        private void ClearInput()
        {
            CategoriName = string.Empty;
            WrongMessage = string.Empty;
        }
        public RelayCommand AddUpdateCategorieCommand => new RelayCommand(AddUpdateCategorie);
        private void AddUpdateCategorie(object parameter)
        {
            if (!InputValidation()) return;
            Categorie newCategorie = new Categorie();
            newCategorie.NumeCategorie = CategoriName;
    
            if (updatedCategori == null)
            {
                categorieBLL.AddCategori(newCategorie);
            }
            else
            {
                newCategorie.CategorieID = updatedCategori.CategorieID;
                categorieBLL.UpdateCategorie(newCategorie);
                BackToCreate(null);
            }
            ClearInput();
            UpdateGuiList(categorieBLL.SearchCategori(SearchValue));
        }
        public RelayCommand DeleteCategorieCommand => new RelayCommand(DeleteCategorie);
        private void DeleteCategorie(object parameter)
        {
            Categorie categorieDel = (parameter as Categorie);
            MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to delete {categorieDel.NumeCategorie} ?", "Delete Categori", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.No) return;
            if (!categorieBLL.DeleteCategorie(categorieDel))
            {
                MessageBox.Show($"You cannot delete because there are products that have it!", "Can't Delete");
            };
            BackToCreate(null);
            UpdateGuiList(categorieBLL.SearchCategori(SearchValue));
        }
        public RelayCommand PressUpdateCommand => new RelayCommand(PressUpdate);
        private void PressUpdate(object parameter)
        {
            updatedCategori = (parameter as Categorie);
            CategoriName = updatedCategori.NumeCategorie;
            WrongMessage = string.Empty;
            CreateUpdate = "Update";
            BacktoCreate = Visibility.Visible;
        }

        public RelayCommand BackToCreateCommand => new RelayCommand(BackToCreate);
        private void BackToCreate(object parameter)
        {
            CreateUpdate = "Create";
            BacktoCreate = Visibility.Collapsed;
            updatedCategori = null;
            ClearInput();
        }

    }
}
