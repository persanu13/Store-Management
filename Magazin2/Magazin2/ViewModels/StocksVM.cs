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
    public class StocksVM : Core.ObservableObject
    {
        private StocBLL stocBLL = new StocBLL();
        private ProdusBLL produsBLL = new ProdusBLL();

        public ObservableCollection<Stoc> StocList { get; set; }
        public ObservableCollection<Produs> AllProduse { get; set; }

        private Produs _selectedProdus;
        public Produs SelectedProdus
        {
            get { return _selectedProdus; }
            set { _selectedProdus = value; OnPropertyChanged(); }
        }
        private int _quantity;
        public string Quantity
        {
            get { return _quantity.ToString(); }
            set 
            {
                int intValue;
                if (string.IsNullOrEmpty(value)) _quantity = 0;
                if (int.TryParse(value, out intValue)) _quantity = intValue;
                OnPropertyChanged();
            }
        }
        private DateTime _supplyDate = DateTime.Now.Date;
        public DateTime SupplyDate { 
            get { return _supplyDate; }
            set { _supplyDate = value; OnPropertyChanged(); } 
        }
        private DateTime _expirationDate = DateTime.Now.Date;
        public DateTime ExpirationDate
        {
            get { return _expirationDate.Date; }
            set { _expirationDate = value; OnPropertyChanged(); }
        }
        private double _purchasePrice;
        public string PurchasePrice
        {
            get { return _purchasePrice.ToString(); }
            set
            {
                double floatValue;
                if (string.IsNullOrEmpty(value)) _purchasePrice = 0;
                if (double.TryParse(value, out floatValue)) _purchasePrice = floatValue;
                OnPropertyChanged();
            }
        }
        private string _wrongMessage = string.Empty;
        public string WrongMessage
        {
            get { return _wrongMessage; }
            set { _wrongMessage = value; OnPropertyChanged(); }
        }
        private Produs _selectedSearchProdus;
        public Produs SelectedSearchProdus
        {
            get { return _selectedSearchProdus; }
            set 
            {   
                _selectedSearchProdus = value;
                UpdateGuiList(stocBLL.SearchStoc(_selectedSearchProdus));
                OnPropertyChanged();
            }
        }

        public StocksVM()
        {
            StocList = stocBLL.GetAllStocuri();
            AllProduse = produsBLL.GetAllProduse();
            SelectedProdus = new Produs() {NumeProdus = "None" };
            SelectedSearchProdus = SelectedProdus;
            AllProduse.Insert(0, SelectedProdus);
        }
        private void UpdateGuiList(ObservableCollection<Stoc> newStocList)
        {
            StocList.Clear();
            foreach (Stoc stoc in newStocList)
            {
                StocList.Add(stoc);
            }
        }

        private bool InputValidation()
        {
            if (SelectedProdus.NumeProdus == "None")
            {
                WrongMessage = "Produs is not selected!";
                return false;
            }
            if (_quantity < 1)
            {
                WrongMessage = "Quantity is 0!";
                return false;
            }
            if (SupplyDate.CompareTo(DateTime.Now.Date) < 0)
            {
                WrongMessage = "Supply date cannot be older than the current date!";
                return false;
            }
            if (ExpirationDate.CompareTo(SupplyDate) < 0 || ExpirationDate.CompareTo(SupplyDate) == 0)
            {
                WrongMessage = "Expiration date cannot be older or same than the supply date!";
                return false;
            }
            if (_purchasePrice < 1)
            {
                WrongMessage = "Purchase price is 0!";
                return false;
            }
            return true;
        }
        private void ClearInput()
        {
            SelectedProdus = AllProduse.First();
            Quantity = string.Empty;
            SupplyDate = DateTime.Now.Date;
            ExpirationDate = DateTime.Now.Date;
            PurchasePrice = string.Empty;
            WrongMessage = string.Empty;
        }
        public RelayCommand AddStocCommand => new RelayCommand(AddStoc);
        private void AddStoc(object parameter)
        {
            if (!InputValidation()) return;
            Stoc newStoc = new Stoc();
            newStoc.ProdusID = SelectedProdus.ProdusID;
            newStoc.Cantitate = _quantity;
            newStoc.DataAprovizionare = SupplyDate;
            newStoc.DataExpirare = ExpirationDate;
            newStoc.PretAchizitie = _purchasePrice;
            stocBLL.AddStoc(newStoc);
            ClearInput();
            UpdateGuiList(stocBLL.SearchStoc(SelectedSearchProdus));
        }


    }
}
