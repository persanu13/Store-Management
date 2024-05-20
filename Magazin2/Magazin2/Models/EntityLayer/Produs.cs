using Magazin2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Produs : Core.ObservableObject
    {
        private int? produsID;
        public int? ProdusID
        {
            get { return produsID; }
            set { produsID = value; OnPropertyChanged(); }
        }
        private int? categorieID;
        public int? CategorieID
        {
            get { return categorieID; }
            set { categorieID = value; OnPropertyChanged(); }
        }
        private int? producatorID;
        public int? ProducatorID
        {
            get { return producatorID; }
            set { producatorID = value; OnPropertyChanged(); }
        }
        private string numeProdus;
        public string NumeProdus
        {
            get { return numeProdus; }
            set { numeProdus = value; OnPropertyChanged(); }
        }
        private string codDeBare;
        public string CodDeBare
        {
            get { return codDeBare; }
            set { codDeBare = value; OnPropertyChanged(); }
        }


    }
}
