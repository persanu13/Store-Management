using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Stoc : Core.ObservableObject
    {
        private int? stocID;
        public int? StocID
        {
            get { return stocID; }
            set { stocID = value; OnPropertyChanged(); }
        }
        private int? produsID;
        public int? ProdusID
        {
            get { return produsID; }
            set { produsID = value; OnPropertyChanged(); }
        }
        private int _cantitate;
        public int Cantitate
        {
            get { return _cantitate; }
            set { _cantitate = value; OnPropertyChanged(); }
        }

        private DateTime _dataAprovizionare;
        public DateTime DataAprovizionare
        {
            get { return _dataAprovizionare; }
            set { _dataAprovizionare = value; OnPropertyChanged(); }
        }
        private DateTime _dataExpirare;
        public DateTime DataExpirare
        {
            get { return _dataExpirare; }
            set { _dataExpirare = value; OnPropertyChanged(); }
        }

        private double _pretAchizitie;
        public double PretAchizitie
        {
            get { return _pretAchizitie; }
            set { _pretAchizitie = value; OnPropertyChanged(); }
        }
        private double _pretVanzare;
        public double PretVanzare
        {
            get { return _pretVanzare; }
            set { _pretVanzare = value; OnPropertyChanged(); }
        }


    }
}
