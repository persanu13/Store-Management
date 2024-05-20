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
    public class ReceiptsVM : Core.ObservableObject
    {
        private BonBLL bonBLL = new BonBLL();
        public ObservableCollection<Bon> BonList {  get; set; }
        private DateTime _selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(); }
        }

        public ReceiptsVM() 
        {
            BonList = bonBLL.GetAllBonuri();
        }


        public RelayCommand ViewDetailsCommand => new RelayCommand(ViewDetails);
  
        private void ViewDetails(object parameter)
        {
            new ReceiptDetailsVM(parameter as Bon);
        }
        public RelayCommand DelteBonCommand => new RelayCommand(DelteBon);
        private void DelteBon(object parameter)
        {
            Bon bonDel = (parameter as Bon);
            MessageBoxResult confirm = MessageBox.Show($"Are you sure you want to delete this receipment ?", "Delete Receipment", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.No) return;
            bonBLL.DeleteBon(bonDel);
            BonList.Clear();
            foreach (Bon bon in bonBLL.GetAllBonuri())
            {
                BonList.Add(bon);
            }
        }
        public RelayCommand MaxBonCommand => new RelayCommand(MaxBon);
        private void MaxBon(object parameter)
        {
            Bon bon = bonBLL.MaxBon(SelectedDate);
            if(bon.BonID == null)
            {
                MessageBox.Show($"Don't exist receipment in {SelectedDate.ToShortDateString()}");
                return;
            }
            ViewDetails(bon);
        }



    }
}
