using Magazin2.Core;
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
    public class ProductsVM : Core.ObservableObject
    {
        private ProdusBLL produsBLL = new ProdusBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();
        private ProducatorBLL producatorBLL = new ProducatorBLL();
        public ObservableCollection<Produs> ProdusList { get; set; }
        public ObservableCollection<Categorie> AllCategories { get; set; }
        public ObservableCollection<Producator> AllProducatori { get; set; }

        private Produs updatedProdus = null;

        private string _produsName = string.Empty;
        public string ProdusName
        {
            get { return _produsName; }
            set { _produsName = value; OnPropertyChanged(); }
        }
        private string _barCode = string.Empty;
        public string BarCode
        {
            get { return _barCode; }
            set 
            {
                if (value.All(char.IsDigit)){
                    _barCode = value;
                }
                OnPropertyChanged();
            }
        }

        private Categorie _selectedCategorie;
        public Categorie SelectedCategorie
        {
            get { return _selectedCategorie; }
            set { _selectedCategorie = value; OnPropertyChanged(); }
        }

        private Producator _selectedProducator;
        public Producator SelectedProducator
        {
            get { return _selectedProducator; }
            set { _selectedProducator = value; OnPropertyChanged(); }
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
                UpdateGuiList(produsBLL.SearchProdus(_searchValue, SelectedSearchCategorie, SelectedSearchProducator));
            }
        }
        private Categorie _selectedSearchCategorie;
        public Categorie SelectedSearchCategorie
        {
            get { return _selectedSearchCategorie; }
            set 
            { 
                _selectedSearchCategorie = value;
                OnPropertyChanged();
                UpdateGuiList(produsBLL.SearchProdus(SearchValue, _selectedSearchCategorie, SelectedSearchProducator));
            }
        }

        private Producator _selectedSearchProducator;
        public Producator SelectedSearchProducator
        {
            get { return _selectedSearchProducator; }
            set 
            { 
                _selectedSearchProducator = value;
                OnPropertyChanged();
                UpdateGuiList(produsBLL.SearchProdus(SearchValue, SelectedSearchCategorie, _selectedSearchProducator));
            }
        }

        public ProductsVM() 
        {
            ProdusList = produsBLL.GetAllProduse();
            AllCategories = categorieBLL.GetAllCategories();
            AllProducatori = producatorBLL.GetAllProducatori();
            SelectedCategorie = new Categorie() {NumeCategorie = "None" };
            SelectedSearchCategorie = SelectedCategorie;
            SelectedProducator = new Producator() { NumeProducator = "None" };
            SelectedSearchProducator = SelectedProducator;
            AllCategories.Insert(0, SelectedCategorie);
            AllProducatori.Insert(0, SelectedProducator);
            CreateUpdate = "Create";
        }

        private void UpdateGuiList(ObservableCollection<Produs> newProdusList)
        {
            ProdusList.Clear();
            foreach (Produs produs in newProdusList)
            {
                ProdusList.Add(produs);
            }
        }

        private bool InputValidation()
        {
            if (string.IsNullOrEmpty(ProdusName))
            {
                WrongMessage = "Produs name is empty!";
                return false;
            }
            if (BarCode.Length != 10)
            {
                WrongMessage = "Barcode is incomplete!";
                return false;
            }
            if(SelectedCategorie.NumeCategorie == "None")
            {
                WrongMessage = "Categori is not selected!!";
                return false;
            }
            if (SelectedProducator.NumeProducator == "None")
            {
                WrongMessage = "Manufacturer is not selected!";
                return false;
            }

            if (produsBLL.ExistProdus(BarCode) && updatedProdus?.CodDeBare != BarCode)
            {
                WrongMessage = "BarCode is already used!";
                return false;
            }
            return true;
        }
        private void ClearInput()
        {
            ProdusName = string.Empty;
            BarCode = string.Empty;
            SelectedCategorie = AllCategories.First();
            SelectedProducator = AllProducatori.First();
            WrongMessage = string.Empty;
        }
        public RelayCommand AddUpdateProdusCommand => new RelayCommand(AddUpdateProdus);
        private void AddUpdateProdus(object parameter)
        {
            if (!InputValidation()) return;
            Produs newProdus = new Produs();
            newProdus.NumeProdus = ProdusName;
            newProdus.CodDeBare = BarCode;
            newProdus.CategorieID = SelectedCategorie.CategorieID;
            newProdus.ProducatorID = SelectedProducator.ProducatorID;
            if (updatedProdus == null)
            {
                produsBLL.AddProdus(newProdus);
            }
            else
            {
                newProdus.ProdusID = updatedProdus.ProdusID;
                produsBLL.UpdateProdus(newProdus);
                BackToCreate(null);
            }
            ClearInput();
            UpdateGuiList(produsBLL.SearchProdus(SearchValue, SelectedSearchCategorie, SelectedSearchProducator));
        }

        public RelayCommand DeleteProdusCommand => new RelayCommand(DeleteProdus);
        private void DeleteProdus(object parameter)
        {
            Produs produsDel = (parameter as Produs);
            MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to delete {produsDel.NumeProdus} ?", "Delete Product", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.No) return;
            produsBLL.DeleteProdus(produsDel);
            BackToCreate(null);
            UpdateGuiList(produsBLL.SearchProdus(SearchValue, SelectedSearchCategorie, SelectedSearchProducator));
        } 
        public RelayCommand PressUpdateCommand => new RelayCommand(PressUpdate);
        private void PressUpdate(object parameter)
        {
            updatedProdus = (parameter as Produs);
            ProdusName = updatedProdus.NumeProdus;
            BarCode = updatedProdus.CodDeBare;
            SelectedCategorie = AllCategories.FirstOrDefault(c => c.CategorieID == updatedProdus.CategorieID);
            SelectedProducator = AllProducatori.FirstOrDefault(p => p.ProducatorID == updatedProdus.ProducatorID);
            WrongMessage = string.Empty;
            CreateUpdate = "Update";
            BacktoCreate = Visibility.Visible;
        }

        public RelayCommand BackToCreateCommand => new RelayCommand(BackToCreate);
        private void BackToCreate(object parameter)
        {
            CreateUpdate = "Create";
            BacktoCreate = Visibility.Collapsed;
            updatedProdus = null;
            ClearInput();
        }


    }
}
