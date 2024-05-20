using Magazin2.Models;
using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Services
{
    public class LoginService
    {
        static public Utilizator LoginSrv(string nume_utilizator,string parola)
        {
            Utilizator returned = null;
            UtilizatorBLL utilizatorBLL = new UtilizatorBLL();
            ObservableCollection<Utilizator> utilizatorList = utilizatorBLL.GetAllUtilizatori();

            foreach(Utilizator utilizator in  utilizatorList)
            {
                if(utilizator.NumeUtilizator ==  nume_utilizator && utilizator.Parola == parola)
                {
                    returned = utilizator;
                }
            }
            return returned;
        }
    }
}
