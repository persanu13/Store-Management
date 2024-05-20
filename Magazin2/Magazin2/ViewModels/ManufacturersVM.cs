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
    public class ManufacturersVM : Core.ObservableObject
    {

        private ProducatorBLL producatorBLL = new ProducatorBLL();
        public ObservableCollection<Producator> ProducatorList { get; set; }

        private Producator updatedProducator = null;
        public List<string> Countries
        {
            get { return Services.CountriesService.Countries; }
        }
       
        private string _producatorName = string.Empty;
        public string ProducatorName
        {
            get { return _producatorName; }
            set { _producatorName = value; OnPropertyChanged(); }
        }

        private string _selectedCountry = Services.CountriesService.Countries[0];
        public string SelectedCountry 
        {
            get { return _selectedCountry; }
            set { _selectedCountry = value; OnPropertyChanged(); }
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
                UpdateGuiList(producatorBLL.SearchProducatori(_searchValue, null));
            }
        }
        private string _selectedSearchCountry = Services.CountriesService.Countries[0];
        public string SelectedSearchCountry
        {
            get { return _selectedSearchCountry; }
            set 
            { 
                _selectedSearchCountry = value;
                OnPropertyChanged();
                UpdateGuiList(producatorBLL.SearchProducatori(SearchValue, SearchCountryValue));
            }
        }
        public string SearchCountryValue
        {
            get
            {
                if (_selectedSearchCountry == "None") return null; 
                return _selectedSearchCountry;
            }
        }

        public ManufacturersVM()
        {
            ProducatorList = producatorBLL.GetAllProducatori();
            CreateUpdate = "Create";
        }

        private void UpdateGuiList(ObservableCollection<Producator> newProducatorList)
        {
            ProducatorList.Clear();
            foreach (Producator producator in newProducatorList)
            {
                ProducatorList.Add(producator);
            }
        }

        private bool InputValidation()
        {
            if (string.IsNullOrEmpty(ProducatorName))
            {
                WrongMessage = "Manufacturer name is empty!";
                return false;
            }
            if (SelectedCountry == "None")
            {
                WrongMessage = "Origin contry is not selected!";
                return false;
            }
            if (producatorBLL.ProducatorExist(ProducatorName) && updatedProducator?.NumeProducator != ProducatorName)
            {
                WrongMessage = "Manufacturer exist!";
                return false;
            }
            return true;
        }
        private void ClearInput()
        {
            ProducatorName = string.Empty;
            SelectedCountry = "None";
            WrongMessage = string.Empty;
        }

        public RelayCommand AddUpdateProducatorCommand => new RelayCommand(AddUpdateProducator);
        private void AddUpdateProducator(object parameter)
        {
            if (!InputValidation()) return;
            Producator newProducator = new Producator();
            newProducator.NumeProducator = ProducatorName;
            newProducator.TaraOrigine = SelectedCountry;
            if (updatedProducator == null)
            {
                producatorBLL.AddProducator(newProducator);
            }
            else
            {
                newProducator.ProducatorID = updatedProducator.ProducatorID;
                producatorBLL.UpdateProducator(newProducator);
                BackToCreate(null);
            }
            ClearInput();
            UpdateGuiList(producatorBLL.SearchProducatori(SearchValue, SearchCountryValue));
        }

        public RelayCommand DeleteProducatorCommand => new RelayCommand(DeleteProducator);
        private void DeleteProducator(object parameter)
        {
            Producator producatorDel = (parameter as Producator);
            MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to delete {producatorDel.NumeProducator} ?", "Delete Manufacturer", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.No) return;
            if (!producatorBLL.DeleteProducator(producatorDel))
            {
                MessageBox.Show($"You cannot delete because there are products that have it!", "Can't Delete");
            };
            BackToCreate(null);
            UpdateGuiList(producatorBLL.SearchProducatori(SearchValue, SearchCountryValue));
        }

        public RelayCommand PressUpdateCommand => new RelayCommand(PressUpdate);
        private void PressUpdate(object parameter)
        {
            updatedProducator = (parameter as Producator);
            ProducatorName = updatedProducator.NumeProducator;
            SelectedCountry= updatedProducator.TaraOrigine;
            WrongMessage = string.Empty;
            CreateUpdate = "Update";
            BacktoCreate = Visibility.Visible;
        }

        public RelayCommand BackToCreateCommand => new RelayCommand(BackToCreate);
        private void BackToCreate(object parameter)
        {
            CreateUpdate = "Create";
            BacktoCreate = Visibility.Collapsed;
            updatedProducator = null;
            ClearInput();
        }

    }
}
