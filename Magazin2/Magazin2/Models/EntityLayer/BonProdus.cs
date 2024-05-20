using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class BonProdus : Core.ObservableObject
    {
        private int? bonID;
        public int? BonID
        {
            get { return bonID; }
            set { bonID = value; OnPropertyChanged(); }
        }
        private int? produsID;
        public int? ProdusID
        {
            get { return produsID; }
            set { produsID = value; OnPropertyChanged(); }
        }
        private int cantitate;
        public int Cantitate
        {
            get { return cantitate; }
            set { cantitate = value; OnPropertyChanged(); }
        }
        private double subTotal;
        public double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; OnPropertyChanged(); }
        }


    }
}
