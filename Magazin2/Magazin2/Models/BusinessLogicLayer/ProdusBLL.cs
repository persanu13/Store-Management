using Magazin2.Models.DataAccessLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.BusinessLogicLayer
{
    public class ProdusBLL
    {
        private ProdusDAL produsDAL = new ProdusDAL();

        public ObservableCollection<Produs> GetAllProduse()
        {
            return produsDAL.GetAllProduse();
        }
        public ObservableCollection<Produs> SearchProdus(string sir, Categorie categorie, Producator producator)
        {
            return produsDAL.SearchProdus(sir, categorie, producator);
        }
        public string GetCategoryName(Produs produs)
        {
            return produsDAL.GetCategoryName(produs);
        }
        public string GetManufacturerName(Produs produs)
        { 
            return produsDAL.GetManufacturerName(produs);
        }
        public void AddProdus(Produs produs)
        {
            produsDAL.AddProdus(produs);
        }
        public void DeleteProdus(Produs produs)
        {
            produsDAL.DeleteProdus(produs);
        }
        public void UpdateProdus(Produs produs)
        {
            produsDAL.UpdateProdus(produs);
        }
        public bool ExistProdus(string codDeBare)
        { 
            return produsDAL.ExistProdus(codDeBare);
        }
        public string GetNumeProductById(int? produsId)
        {
            return produsDAL.GetNumeProductById(produsId);
        }

    }
}
