using Magazin2.Services;
using Magazin2.Models.EntityLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Magazin2.Models.DataAccessLayer
{
    public class StocDAL
    {
        public ObservableCollection<Stoc> GetAllStocuri()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectStocuri", con);
                ObservableCollection<Stoc> result = new ObservableCollection<Stoc>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Stoc stoc = new Stoc();
                    stoc.StocID = (int)(reader[0]);
                    stoc.ProdusID = (int)(reader[1]);
                    stoc.Cantitate = (int)(reader[2]);
                    stoc.DataAprovizionare = (DateTime)reader[3];
                    stoc.DataExpirare = (DateTime)reader[4];
                    stoc.PretAchizitie = (double)reader[5];
                    stoc.PretVanzare = (double)reader[6];
                    result.Add(stoc);
                }
                reader.Close();
                return result;
            }
        }
        public ObservableCollection<Stoc> SearchStoc(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Stoc> result = new ObservableCollection<Stoc>();
                SqlCommand cmd = new SqlCommand("FiltreazaStocuri", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@produs_id", (object)produs?.ProdusID ?? DBNull.Value);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Stoc stoc = new Stoc();
                    stoc.StocID = (int)(reader[0]);
                    stoc.ProdusID = (int)(reader[1]);
                    stoc.Cantitate = (int)(reader[2]);
                    stoc.DataAprovizionare = (DateTime)reader[3];
                    stoc.DataExpirare = (DateTime)reader[4];
                    stoc.PretAchizitie = (double)reader[5];
                    stoc.PretVanzare = (double)reader[6];
                    result.Add(stoc);
                }
                reader.Close();
                return result;
            }
        }

        public ObservableCollection<Stoc> SearchStocForCashier(string numeProdus,string codDeBare,
            int? categorieId,int? producatorId,DateTime? dataExpirare)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Stoc> result = new ObservableCollection<Stoc>();
                SqlCommand cmd = new SqlCommand("FiltrareStocuriPentruCasier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nume_produs", (object)numeProdus ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cod_de_bare", (object)codDeBare ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categorie_id", (object)categorieId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@producator_id", (object)producatorId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@data_expirare", (object)dataExpirare ?? DBNull.Value);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Stoc stoc = new Stoc();
                    stoc.StocID = (int)(reader[0]);
                    stoc.ProdusID = (int)(reader[1]);
                    stoc.Cantitate = (int)(reader[2]);
                    stoc.DataAprovizionare = (DateTime)reader[3];
                    stoc.DataExpirare = (DateTime)reader[4];
                    stoc.PretAchizitie = (double)reader[5];
                    stoc.PretVanzare = (double)reader[6];
                    result.Add(stoc);
                }
                reader.Close();
                return result;
            }
        }

        public Produs GetProductForStoc(Stoc stoc)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectProdusStoc", con);
                Produs produs = new Produs();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stoc_id", stoc.StocID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    produs.ProdusID = (int)(reader[0]);
                    produs.CategorieID = (int)(reader[1]);
                    produs.ProducatorID = (int)(reader[2]);
                    produs.NumeProdus = reader.GetString(3);
                    produs.CodDeBare = reader.GetString(4);
                }
                reader.Close();
                return produs;
            }
        }


        public void AddStoc(Stoc stoc)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("AdaugaStoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@produs_id", stoc.ProdusID);
                cmd.Parameters.AddWithValue("@cantitate", stoc.Cantitate);
                cmd.Parameters.AddWithValue("@data_aprovizionare", stoc.DataAprovizionare);
                cmd.Parameters.AddWithValue("@data_expirare", stoc.DataExpirare);
                cmd.Parameters.AddWithValue("@pret_achizitie", stoc.PretAchizitie);
                double pretVanzare = stoc.PretAchizitie + stoc.PretAchizitie * DbService.CommercialAddition;
                cmd.Parameters.AddWithValue("@pret_vanzare", pretVanzare);
                SqlParameter idStocParam = new SqlParameter("@stoc_id", SqlDbType.Int);
                idStocParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(idStocParam);
                con.Open();
                cmd.ExecuteNonQuery();
                stoc.StocID = idStocParam.Value as int?;
            }
        }
        public void DeleteStoc(Stoc stoc)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaStoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stoc_id", stoc.StocID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateQuantityStoc(Stoc stoc)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ActualizareCantitateStoc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stoc_id", stoc.StocID);
                cmd.Parameters.AddWithValue("@cantitate", stoc.Cantitate);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
