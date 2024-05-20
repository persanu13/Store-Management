using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Services
{
    public class MiniServices
    {
        public static bool DigitsOnly(string numar)
        {
            foreach (char c in numar)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
