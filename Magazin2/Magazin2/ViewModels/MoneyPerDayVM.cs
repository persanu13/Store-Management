using Magazin2.Core;
using Magazin2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazin2.ViewModels
{
    public class MoneyPerDayVM
    {
        public ObservableCollection<Tuple<int, double>> DayList { get; set; }
        public DateTime? Date { get; set; }
        public string CashierName { get; set; }
        public MoneyPerDayVM(ObservableCollection<Tuple<int, double>> dayList, DateTime? date, string cashierName) 
        {
            DayList = dayList;
            Date = date;
            CashierName = cashierName;
            Window newWindow = new MoneyPerDayView();
            newWindow.DataContext = this;
        }


    }
}
