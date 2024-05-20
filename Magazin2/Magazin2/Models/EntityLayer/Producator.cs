using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Producator : Core.ObservableObject
    {
        private int? producatorID;
        public int? ProducatorID
        {
            get { return producatorID; }
            set { producatorID = value; OnPropertyChanged(); }
        }
        private string numeProducator;
        public string NumeProducator
        {
            get { return numeProducator; }
            set { numeProducator = value; OnPropertyChanged(); }
        }
        private string taraOrigine;
        public string TaraOrigine
        {
            get { return taraOrigine; }
            set { taraOrigine = value; OnPropertyChanged(); }
        }
    }
}
