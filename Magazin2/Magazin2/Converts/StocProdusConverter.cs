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
        private ProdusBLL produsBLL = new ProdusBLL();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Stoc))
                return null;

            Stoc stoc = (Stoc)value;
            Produs produs = stocBLL.GetProductForStoc(stoc);
            if (parameter.ToString() == "NumeProdus") return produs.NumeProdus;
            if (parameter.ToString() == "CodDeBare") return produs.CodDeBare;
            if (parameter.ToString() == "Categorie") return produsBLL.GetCategoryName(produs);
            if (parameter.ToString() == "Producator") return produsBLL.GetManufacturerName(produs);
            return produs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
