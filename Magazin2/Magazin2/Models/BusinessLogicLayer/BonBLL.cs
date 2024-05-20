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
    public class BonBLL
    {
        private BonDAL bonDal = new BonDAL();

        public ObservableCollection<Bon> GetAllBonuri()
        {
            return bonDal.GetAllBonuri();   
        }
        public string GetUserName(Bon bon)
        {
            return bonDal.GetUserName(bon);
        }
        public List<BonProdus> GetReceiptDetails(Bon bon)
        {
            return bonDal.GetReceiptDetails(bon);
        }
        public void DeleteBon(Bon bon)
        {
            bonDal.DeleteBon(bon);
        }
        public Bon MaxBon(DateTime date)
        {
            return bonDal.MaxBon(date);
        }



        }
}
