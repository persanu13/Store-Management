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
    public class CasierPageVM : Core.ObservableObject
    {
        private BonBLL bonBLL = new BonBLL();
        private StocBLL stocBLL = new StocBLL();
        private CategorieBLL categorieBLL = new CategorieBLL();
        private ProducatorBLL producatorBLL = new ProducatorBLL();
        public ObservableCollection<Stoc> StocList { get; set; }
        public ObservableCollection<Categorie> AllCategories { get; set; }
        public ObservableCollection<Producator> AllProducatori { get; set; }
        private Utilizator currentUser;
        private HashSet<Stoc> updatedStocuri = new HashSet<Stoc>();

        private string _produsName = string.Empty;
        public string ProdusName
        {
            get { return _produsName; }
            set 
            {
                _produsName = value;
                OnPropertyChanged();
                UpdateStocList();
            }
        }
        private string _barCode = string.Empty;
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                if (value.All(char.IsDigit))
                {
                    _barCode = value;
                    UpdateStocList();
                }
                OnPropertyChanged();
            }
        }
        private Categorie _selectedCategorie;
        public Categorie SelectedCategorie
        {
            get { return _selectedCategorie; }
            set 
            {
                _selectedCategorie = value;
                OnPropertyChanged();
                UpdateStocList();
            }
        }

        private Producator _selectedProducator;
        public Producator SelectedProducator
        {
            get { return _selectedProducator; }
            set 
            { _selectedProducator = value;
                OnPropertyChanged();
                UpdateStocList();
            }
        }

        private DateTime? _selectedDate = DateTime.Now;
        public DateTime? SelectedDate
        {
            get {
                if (!DateEnable) return null;
                return _selectedDate;
            }
            set 
            {
                _selectedDate = value;
                OnPropertyChanged();
                UpdateStocList();
            }
        }
        private bool _dateEnable;
        public bool DateEnable
        {
            get { return _dateEnable; }
            set
            { 
                _dateEnable = value;
                OnPropertyChanged();
                SelectedDate = _selectedDate;
                UpdateStocList();
            }
        }

        private int _quantity = 1;
        public string Quantity
        {
            get { return _quantity.ToString(); }
            set
            {
                int intValue;
                if (string.IsNullOrEmpty(value) || (value == "0")) _quantity = 1;
                else
                {
                    if (int.TryParse(value, out intValue)) _quantity = intValue;
                }
                OnPropertyChanged();
            }
        }

        private Bon _createdBon;
        public Bon CreatedBon
        {
            get { return _createdBon; }
            set { _createdBon = value; OnPropertyChanged(); }
        }

        public CasierPageVM(Utilizator utilizator)
        {
            currentUser = utilizator;
            StocList = new ObservableCollection<Stoc>();
            InitNewBon();
            AllCategories = categorieBLL.GetAllCategories();
            AllProducatori = producatorBLL.GetAllProducatori();
            SelectedCategorie = new Categorie() { NumeCategorie = "None" };
            SelectedProducator = new Producator() { NumeProducator = "None" };
            AllCategories.Insert(0, SelectedCategorie);
            AllProducatori.Insert(0, SelectedProducator);
            UpdateStocList();
        }

        private void UpdateStocList()
        {
            StocList.Clear();
            foreach (Stoc stoc in stocBLL.SearchStocForCashier(ProdusName,BarCode,
                SelectedCategorie?.CategorieID,SelectedProducator?.ProducatorID,SelectedDate)) 
            { 
                StocList.Add(stoc);
            }
        }
        private void InitNewBon()
        {
            CreatedBon = new Bon();
            CreatedBon.UtilizatorID = currentUser.UtilizatorID;
            CreatedBon.NumarBon = bonBLL.GetBonNumberToday() + 1;
            CreatedBon.ListaProduse = new ObservableCollection<BonProdus>();
            CreatedBon.SumaTotala = 0;
            
        }

        public RelayCommand PressPlusCommand => new RelayCommand(PressPlus);
        private void PressPlus(object parameter)
        {
            Stoc stoc = (parameter as Stoc);
            if (stoc.Cantitate < _quantity)
            {
                MessageBox.Show($"We don't have enought product in stock, max stock is {stoc.Cantitate}!");
                return;
            }
            updatedStocuri.Add(stoc);
            BonProdus existBonProdus = existProductInBon(stoc.ProdusID, CreatedBon);
            if (existBonProdus == null) 
            {
                BonProdus bonProdus = new BonProdus();
                bonProdus.ProdusID = stoc.ProdusID;
                bonProdus.Cantitate = _quantity;
                stoc.Cantitate -= _quantity;
                bonProdus.SubTotal = _quantity * stoc.PretVanzare;
                CreatedBon.SumaTotala += _quantity * stoc.PretVanzare;
                CreatedBon.ListaProduse.Add(bonProdus);
            }
            else
            {
                existBonProdus.Cantitate += _quantity;
                stoc.Cantitate -= _quantity;
                existBonProdus.SubTotal += _quantity * stoc.PretVanzare;
                CreatedBon.SumaTotala += _quantity * stoc.PretVanzare;
            }
            Quantity = string.Empty;
        }

        private BonProdus existProductInBon(int? produsId, Bon bon)
        {
            foreach(BonProdus bonProdus in bon.ListaProduse)
            {
                if(bonProdus.ProdusID == produsId) return bonProdus;
            }
            return null;
        }

        public RelayCommand PressMinusCommand => new RelayCommand(PressMinus);
        private void PressMinus(object parameter)
        {
            BonProdus bonProdus = (parameter as BonProdus);
            CreatedBon.SumaTotala -= bonProdus.SubTotal;
            Stoc stocScos = updatedStocuri.Where(s => s.ProdusID == bonProdus.ProdusID).ToList().First();
            stocScos.Cantitate += bonProdus.Cantitate;
            updatedStocuri.Remove(stocScos);
            CreatedBon.ListaProduse.Remove(bonProdus);
        }
        public RelayCommand PressOkCommand => new RelayCommand(PressOk);
        private void PressOk(object parameter)
        {
            bonBLL.AddBon(CreatedBon);
            CreatedBon.DataEliberare = DateTime.Now;
            foreach (Stoc stoc in updatedStocuri)
            {
                stocBLL.UpdateQuantityStoc(stoc);
            }
            updatedStocuri = new HashSet<Stoc>();
            ProdusName = string.Empty;
            BarCode = string.Empty;
            SelectedCategorie = AllCategories.FirstOrDefault();
            SelectedProducator = AllProducatori.FirstOrDefault();
            SelectedDate = DateTime.Now;
            DateEnable = false;
            new ReceiptDetailsVM(CreatedBon);
            InitNewBon();

        }

    }
}
