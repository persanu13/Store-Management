using Magazin2.Services;
using Magazin2.Models.EntityLayer;
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
    public class ProdusDAL
    {

        public ObservableCollection<Produs> GetAllProduse()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectProduse", con);
                ObservableCollection<Produs> result = new ObservableCollection<Produs>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Produs produs = new Produs();
                    produs.ProdusID = (int)(reader[0]);
                    produs.CategorieID = (int)(reader[1]);
                    produs.ProducatorID = (int)(reader[2]);
                    produs.NumeProdus = reader.GetString(3);
                    produs.CodDeBare = reader.GetString(4);
                    result.Add(produs);
                }
                reader.Close();
                return result;
            }
        }
        public ObservableCollection<Produs> SearchProdus(string sir,Categorie categorie,Producator producator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Produs> result = new ObservableCollection<Produs>();
                SqlCommand cmd = new SqlCommand("FiltreazaProduse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sir", sir);
                cmd.Parameters.AddWithValue("@categorie_id", (object)categorie?.CategorieID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@producator_id", (object)producator?.ProducatorID ?? DBNull.Value);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Produs produs = new Produs();
                    produs.ProdusID = (int)(reader[0]);
                    produs.CategorieID = (int)(reader[1]);
                    produs.ProducatorID = (int)(reader[2]);
                    produs.NumeProdus = reader.GetString(3);
                    produs.CodDeBare = reader.GetString(4);
                    result.Add(produs);
                }
                reader.Close();
                return result;
            }
        }
        public string GetCategoryName(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectNumeCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter produsIdParam = new SqlParameter("@produs_id",  produs.ProdusID);
                SqlParameter numeCategorieParam = new SqlParameter("@nume_categorie", SqlDbType.NVarChar, 10);
                numeCategorieParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(produsIdParam);
                cmd.Parameters.Add(numeCategorieParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (string)numeCategorieParam.Value;
            }
        }
        public string GetManufacturerName(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectNumeProducator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter produsIdParam = new SqlParameter("@produs_id", produs.ProdusID);
                SqlParameter numeProducatorParam = new SqlParameter("@nume_producator", SqlDbType.NVarChar, 50);
                numeProducatorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(produsIdParam);
                cmd.Parameters.Add(numeProducatorParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (string)numeProducatorParam.Value;
            }
        }
        public void AddProdus(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("AdaugaProdus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter categorieIdParam = new SqlParameter("@categorie_id", produs.CategorieID);
                SqlParameter producatorIdParam = new SqlParameter("@producator_id", produs.ProducatorID);
                SqlParameter numeProdusParam = new SqlParameter("@nume_produs", produs.NumeProdus);
                SqlParameter codDeBareParam = new SqlParameter("@cod_de_bare", produs.CodDeBare);
                SqlParameter produsIdParam = new SqlParameter("@produs_id", SqlDbType.Int);
                produsIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(categorieIdParam);
                cmd.Parameters.Add(producatorIdParam);
                cmd.Parameters.Add(numeProdusParam);
                cmd.Parameters.Add(codDeBareParam);
                cmd.Parameters.Add(produsIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
                produs.ProdusID = produsIdParam.Value as int?;
            }
        }
        public bool DeleteProdus(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaProdus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter produsIdParam = new SqlParameter("@produs_id", produs.ProdusID);
                SqlParameter resultParam = new SqlParameter("@result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(resultParam);
                cmd.Parameters.Add(produsIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)resultParam.Value;
            }
        }
        public void UpdateProdus(Produs produs)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ActualizeazaProdus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@produs_id", produs.ProdusID);
                cmd.Parameters.AddWithValue("@categorie_id", produs.CategorieID);
                cmd.Parameters.AddWithValue("@producator_id", produs.ProducatorID);
                cmd.Parameters.AddWithValue("@nume_produs", produs.NumeProdus);
                cmd.Parameters.AddWithValue("@cod_de_bare", produs.CodDeBare);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ExistProdus(string codDeBare)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("VerificaExistaCodDeBareProdus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_de_bare", codDeBare);
                SqlParameter existaParam = new SqlParameter("@exista", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(existaParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)existaParam.Value;
            }
        }
        public string GetNumeProductById(int? produsId)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectNumeProdusById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter produsIdParam = new SqlParameter("@produs_id", produsId);
                SqlParameter numeProdusParam = new SqlParameter("@nume_produs", SqlDbType.NVarChar, 100);
                numeProdusParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(produsIdParam);
                cmd.Parameters.Add(numeProdusParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (string)numeProdusParam.Value;
            }
        }



    }
}
