using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.EntityLayer
{
    public class Categorie : Core.ObservableObject
    {
        private int? categorieID;
        public int? CategorieID
        {
            get { return categorieID; }
            set { categorieID = value; OnPropertyChanged(); }
        }
        private string numeCategorie;
        public string NumeCategorie
        {
            get { return numeCategorie; }
            set { numeCategorie = value; OnPropertyChanged(); }
        }
    }
}
