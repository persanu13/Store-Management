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
    public class ProducatorBLL
    {
        private ProducatorDAL producatorDal = new ProducatorDAL();
        public ObservableCollection<Producator> GetAllProducatori()
        {
           return producatorDal.GetAllProducatori();
        }

        public ObservableCollection<Producator> SearchProducatori(string sir, string originCountry)
        {
            return producatorDal.SearchProducatori(sir, originCountry);
        }
        public void AddProducator(Producator producator)
        {
            producatorDal.AddProducator(producator);
        }
        public bool DeleteProducator(Producator producator)
        {
            return producatorDal.DeleteProducator(producator);
        }
        public void UpdateProducator(Producator producator)
        {
            producatorDal.UpdateProducator(producator);
        }
        public bool ProducatorExist(string producatorName)
        {
            return producatorDal.ProducatorExist(producatorName);
        }

    }
}
