using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Utilizator : Core.ObservableObject
    {
        private int? utilizatorID;
        public int? UtilizatorID
        {
            get { return utilizatorID; }
            set { utilizatorID = value; OnPropertyChanged(); }
        }

        private string numeUtilizator;
        public string NumeUtilizator
        {
            get { return numeUtilizator; }
            set { numeUtilizator = value; OnPropertyChanged(); }
        }

        private string parola;
        public string Parola
        {
            get { return parola; }
            set { parola = value; OnPropertyChanged(); }
        }

        private string tipUtilizator;
        public string TipUtilizator
        {
            get { return tipUtilizator; }
            set { tipUtilizator = value; OnPropertyChanged(); }
        }

    }
}
