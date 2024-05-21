using System.Configuration;
using System.Data.SqlClient;

namespace Magazin2.Services
{
    public class DbService
    {
        private static readonly string connectionString = "Data Source=STEMI-MACHINE;Initial Catalog=Magazin2DB;Integrated Security=SSPI;";
        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        public static double CommercialAddition
        {
            get { return (double)0.25; }
        }
    }
}
