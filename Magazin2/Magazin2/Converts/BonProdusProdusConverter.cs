using Magazin2.Models.BusinessLogicLayer;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Magazin2.Converts
{
    public class BonProdusProdusConverter : IValueConverter
    {
        private ProdusBLL produsBLL = new ProdusBLL();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is BonProdus))
                return null;

            BonProdus bonProdus = (BonProdus)value;
            return produsBLL.GetNumeProductById(bonProdus.ProdusID);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
