using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.EntityLayer;
using Magazin2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazin2.ViewModels
{
    public class ReceiptDetailsVM : Core.ObservableObject
    {
        private BonBLL bonBLL = new BonBLL();
        private Bon _thisBon;
        public Bon ThisBon { get { return _thisBon; }}
        public ReceiptDetailsVM(Bon bon)
        {
            _thisBon = bon;
            _thisBon.ListaProduse = bonBLL.GetReceiptDetails(_thisBon);
            Window newWindow = new ReceiptDetailsView();
            newWindow.DataContext = this;
        }


    }
}
