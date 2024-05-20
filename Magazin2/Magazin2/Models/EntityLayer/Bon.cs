using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Bon : Core.ObservableObject
    {
        private int? _bonID;
        public int? BonID
        {
            get { return _bonID; }
            set { _bonID = value; OnPropertyChanged(); }
        }
        private int? utilizatorID;
        public int? UtilizatorID
        {
            get { return utilizatorID; }
            set { utilizatorID = value; OnPropertyChanged(); }
        }
        private int _numarBon;
        public int NumarBon
        {
            get { return _numarBon; }
            set { _numarBon = value; OnPropertyChanged(); }
        }

        private DateTime _dataEliberare = DateTime.Now;
        public DateTime DataEliberare
        {
            get { return _dataEliberare; }
            set { _dataEliberare = value; OnPropertyChanged(); }
        }
        private List<BonProdus> _listaProduse;
        public List<BonProdus> ListaProduse
        { 
            get { return _listaProduse; }
            set { _listaProduse = value; OnPropertyChanged(); }
        }
        private double _sumaTotala;
        public double SumaTotala
        {
            get { return _sumaTotala; }
            set { _sumaTotala = value; OnPropertyChanged(); }
        }



    }
}
