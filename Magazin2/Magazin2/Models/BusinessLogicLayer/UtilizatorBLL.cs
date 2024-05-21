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
    public class UtilizatorBLL
    {
        UtilizatorDAL utilizatorDAL = new UtilizatorDAL();

        public ObservableCollection<Utilizator> GetAllUtilizatori()
        {
            return utilizatorDAL.GetAllUtilizatori();
        }
        public ObservableCollection<Utilizator> SearchUtilizatori(string sir, string type)
        {
            return utilizatorDAL.SearchUtilizatori(sir, type);
        }
        public void AddUtilizator(Utilizator utilizator)
        {
            utilizatorDAL.AddUtilizator(utilizator);
        }

        public void DeleteUtilizator(Utilizator utilizator)
        {
            utilizatorDAL.DeleteUtilizator(utilizator);
        }
        public void UpdateUtilizator(Utilizator utilizator)
        {
            utilizatorDAL.UpdateUtilizator(utilizator);
        }

        public bool UserNameExist(string userName)
        {
            return utilizatorDAL.UserNameExist(userName);
        }
        public bool UserPasswordExist(string password)
        {
            return utilizatorDAL.UserPasswordExist(password);
        }
        public ObservableCollection<Tuple<int, double>> TotalMoneyPerDay(int? utilizatorId, DateTime? date)
        {
            return utilizatorDAL.TotalMoneyPerDay(utilizatorId, date);
        }



    }
}
