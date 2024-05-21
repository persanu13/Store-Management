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
    public class StocBLL
    {
        private StocDAL stocDAL = new StocDAL();
        public ObservableCollection<Stoc> GetAllStocuri()
        {
            return stocDAL.GetAllStocuri();
        }
        public Produs GetProductForStoc(Stoc stoc)
        {
            return stocDAL.GetProductForStoc(stoc);
        }
        public ObservableCollection<Stoc> SearchStoc(Produs produs)
        {
            return stocDAL.SearchStoc(produs);
        }
        public ObservableCollection<Stoc> SearchStocForCashier(string numeProdus, string codDeBare,
           int? categorieId, int? producatorId, DateTime? dataExpirare)
        {
            return stocDAL.SearchStocForCashier(numeProdus, codDeBare, categorieId, producatorId, dataExpirare);
        }
        public void AddStoc(Stoc stoc)
        {
            stocDAL.AddStoc(stoc);
        }
        public void UpdateQuantityStoc(Stoc stoc)
        {
            stocDAL.UpdateQuantityStoc(stoc);
        }

    }
}
