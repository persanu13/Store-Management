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
    public class StocProdusConverter : IValueConverter
    {
        private StocBLL stocBLL = new StocBLL();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Stoc))
                return null;

            Stoc stoc = (Stoc)value;
            if (parameter.ToString() == "NumeProdus") return stocBLL.GetProductForStoc(stoc).NumeProdus;
            if (parameter.ToString() == "CodDeBare") return stocBLL.GetProductForStoc(stoc).CodDeBare;
            return stocBLL.GetProductForStoc(stoc);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
