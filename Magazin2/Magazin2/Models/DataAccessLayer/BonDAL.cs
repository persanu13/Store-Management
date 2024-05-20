using Magazin2.Models.EntityLayer;
using Magazin2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin2.Models.DataAccessLayer
{
    public class BonDAL
    {
        public ObservableCollection<Bon> GetAllBonuri()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectBonuri", con);
                ObservableCollection<Bon> result = new ObservableCollection<Bon>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Bon bon = new Bon();
                    bon.BonID = (int)(reader[0]);
                    bon.UtilizatorID = (int)(reader[1]);
                    bon.NumarBon = (int)(reader[2]);
                    bon.DataEliberare = (DateTime)reader[3];
                    bon.SumaTotala = (double)(reader[4]);
                    result.Add(bon);
                }
                reader.Close();
                return result;
            }
        }
        public string GetUserName(Bon bon)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectNumeUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter produsIdParam = new SqlParameter("@bon_id", bon.BonID);
                SqlParameter numeUtilizatorParam = new SqlParameter("@nume_utilizator", SqlDbType.NVarChar, 50);
                numeUtilizatorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(produsIdParam);
                cmd.Parameters.Add(numeUtilizatorParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (string)numeUtilizatorParam.Value;
            }
        }
        public List<BonProdus> GetReceiptDetails(Bon bon)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectDetaliBon", con);
                List<BonProdus> result = new List<BonProdus>();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bon_id", bon.BonID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BonProdus detaliBon = new BonProdus();
                    detaliBon.BonID = (int)(reader[0]);
                    detaliBon.ProdusID = (int)(reader[1]);
                    detaliBon.Cantitate = (int)(reader[2]);
                    detaliBon.SubTotal = (double)(reader[3]);
                    result.Add(detaliBon);
                }
                reader.Close();
                return result;
            }
        }
        public void DeleteBon(Bon bon)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaBon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bon_id", bon.BonID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Bon MaxBon(DateTime date)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectMaxSumBonForDate", con);
                Bon bon = new Bon();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@data_selectata", date);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bon.BonID = (int)(reader[0]);
                    bon.UtilizatorID = (int)(reader[1]);
                    bon.NumarBon = (int)(reader[2]);
                    bon.DataEliberare = (DateTime)reader[3];
                    bon.SumaTotala = (double)(reader[4]);
                }
                reader.Close();
                return bon;
            }
        }


    }
}
