using Magazin2.Models.DataAccessLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazin2.Models.BusinessLogicLayer
{
    public class BonBLL
    {
        private BonDAL bonDAL = new BonDAL();

        public ObservableCollection<Bon> GetAllBonuri()
        {
            return bonDAL.GetAllBonuri();   
        }
        public string GetUserName(Bon bon)
        {
            return bonDAL.GetUserName(bon);
        }
        public ObservableCollection<BonProdus> GetReceiptDetails(Bon bon)
        {
            return bonDAL.GetReceiptDetails(bon);
        }
        public void AddBon(Bon bon)
        {
            bonDAL.AddBon(bon);
            foreach(BonProdus bonProdus in bon.ListaProduse)
            {
                bonProdus.BonID = bon.BonID;
                bonDAL.AddBonProdus(bonProdus);
            }
        }
        public void DeleteBon(Bon bon)
        {
            bonDAL.DeleteBon(bon);
        }
        public Bon MaxBon(DateTime date)
        {
            return bonDAL.MaxBon(date);
        }
        public int GetBonNumberToday()
        {
            return bonDAL.GetBonNumberToday();
        }


    }
}
