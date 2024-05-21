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
    public class CategorieConverter : IValueConverter
    {
        private CategorieBLL categorieBLL = new CategorieBLL();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Categorie))
                return null;
            string operation = (string)parameter;

            Categorie categorie = (Categorie)value;
            if(operation == "sumTotal") return categorieBLL.GetTotalSumByCategory(categorie);
            return "Converter Eror";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
