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

namespace Magazin2.Models.DataAccessLayer
{
    public class CategorieDAL
    {
        public ObservableCollection<Categorie> GetAllCategories()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectCategorii", con);
                ObservableCollection<Categorie> result = new ObservableCollection<Categorie>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Categorie categorie = new Categorie();
                    categorie.CategorieID = (int)(reader[0]);
                    categorie.NumeCategorie = reader.GetString(1);
                    result.Add(categorie);
                }
                reader.Close();
                return result;
            }
        }
        public ObservableCollection<Categorie> SearchCategori(string sir)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Categorie> result = new ObservableCollection<Categorie>();
                SqlCommand cmd = new SqlCommand("CautaCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sirParam = new SqlParameter("@sir_caractere", sir);
                cmd.Parameters.Add(sirParam);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Categorie categorie = new Categorie();
                    categorie.CategorieID = (int)(reader[0]);
                    categorie.NumeCategorie = reader.GetString(1);
                    result.Add(categorie);
                }
                reader.Close();
                return result;
            }
        }
        public void AddCategori(Categorie categorie)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("AdaugaCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeCategorieParam = new SqlParameter("@nume_categorie", categorie.NumeCategorie);
                SqlParameter categorieIdParam = new SqlParameter("@categorie_id", SqlDbType.Int);
                categorieIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeCategorieParam);
                cmd.Parameters.Add(categorieIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
                categorie.CategorieID = categorieIdParam.Value as int?;
            }
        }
        public bool DeleteCategorie(Categorie categorie)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter categorieIdParam = new SqlParameter("@categorie_id", categorie.CategorieID);
                SqlParameter resultParam = new SqlParameter("@result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(categorieIdParam);
                cmd.Parameters.Add(resultParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)resultParam.Value;
            }
        }
        public void UpdateCategorie(Categorie categorie)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ActualizeazaCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter categorieIdParam = new SqlParameter("@categorie_id", categorie.CategorieID);
                SqlParameter numeCategorieParam = new SqlParameter("@nume_categorie", categorie.NumeCategorie);
                cmd.Parameters.Add(numeCategorieParam);
                cmd.Parameters.Add(categorieIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool CategoryExist(string categorieName)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ExistaCategorie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeCategorieParam = new SqlParameter("@nume_categorie", categorieName);
                SqlParameter existaParam = new SqlParameter("@exista", SqlDbType.Bit);
                existaParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeCategorieParam);
                cmd.Parameters.Add(existaParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)existaParam.Value;
            }
        }









    }
}
