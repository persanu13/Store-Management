using Magazin2.Core;
using Magazin2.Models.DataAccessLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.BusinessLogicLayer
{
    public class CategorieBLL
    {
        private CategorieDAL categorieDAL = new CategorieDAL();
        public ObservableCollection<Categorie> GetAllCategories()
        {
            return categorieDAL.GetAllCategories();
        }
        public ObservableCollection<Categorie> SearchCategori(string sir)
        {
            return categorieDAL.SearchCategori(sir);
        }
        public void AddCategori(Categorie categorie)
        {
            categorieDAL.AddCategori(categorie);
        }
        public bool DeleteCategorie(Categorie categorie)
        {
            return categorieDAL.DeleteCategorie(categorie);
        }
        public void UpdateCategorie(Categorie categorie)
        {
            categorieDAL.UpdateCategorie(categorie);
        }
        public bool CategoryExist(string categorieName)
        {
            return categorieDAL.CategoryExist(categorieName);
        }
    }
}
