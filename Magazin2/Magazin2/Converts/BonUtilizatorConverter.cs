using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Magazin2.Converts
{
    public class BonUtilizatorConverter : IValueConverter
    {
        private BonBLL bonBLL = new BonBLL();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Bon))
                return null;

            Bon bon = (Bon)value;
            
            return bonBLL.GetUserName(bon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }



    }
}
