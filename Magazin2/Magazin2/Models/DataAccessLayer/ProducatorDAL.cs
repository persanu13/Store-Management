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
    public class ProducatorDAL
    {
        public ObservableCollection<Producator> GetAllProducatori()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectProducatori", con);
                ObservableCollection<Producator> result = new ObservableCollection<Producator>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Producator producator = new Producator();
                    producator.ProducatorID = (int)(reader[0]);
                    producator.NumeProducator = reader.GetString(1);
                    producator.TaraOrigine = reader.GetString(2);
                    result.Add(producator);
                }
                reader.Close();
                return result;
            }
        }

        public ObservableCollection<Producator> SearchProducatori(string sir, string originCountry)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Producator> result = new ObservableCollection<Producator>();
                SqlCommand cmd = new SqlCommand("FiltreazaProducatori", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sirParam = new SqlParameter("@sir_caractere", sir);
                SqlParameter originCountryParam = new SqlParameter();
                originCountryParam.ParameterName = "@tara_origine";
                if (string.IsNullOrEmpty(originCountry))
                {
                    originCountryParam.Value = DBNull.Value;
                }
                else
                {
                    originCountryParam.Value = originCountry;
                }
                cmd.Parameters.Add(sirParam);
                cmd.Parameters.Add(originCountryParam);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Producator producator = new Producator();
                    producator.ProducatorID = (int)(reader[0]);
                    producator.NumeProducator = reader.GetString(1);
                    producator.TaraOrigine = reader.GetString(2);
                    result.Add(producator);
                }
                reader.Close();
                return result;
            }
        }

        public void AddProducator(Producator producator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("AdaugaProducator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeProducatorParam = new SqlParameter("@nume_producator", producator.NumeProducator);
                SqlParameter taraOrigineParam = new SqlParameter("@tara_origine", producator.TaraOrigine);
                SqlParameter producatorIdParam = new SqlParameter("@producator_id", SqlDbType.Int);
                producatorIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeProducatorParam);
                cmd.Parameters.Add(taraOrigineParam);
                cmd.Parameters.Add(producatorIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
                producator.ProducatorID = producatorIdParam.Value as int?;
            }
        }
        public bool DeleteProducator(Producator producator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaProducator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter categorieIdParam = new SqlParameter("@producator_id", producator.ProducatorID);
                SqlParameter resultParam = new SqlParameter("@result", SqlDbType.Bit);
                resultParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(categorieIdParam);
                cmd.Parameters.Add(resultParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)resultParam.Value;
            }
        }
        public void UpdateProducator(Producator producator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ActualizeazaProducator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter producatorIdParam = new SqlParameter("@producator_id", producator.ProducatorID);
                SqlParameter numeProducatorParam = new SqlParameter("@nume_producator", producator.NumeProducator);
                SqlParameter taraOrigineParam = new SqlParameter("@tara_origine", producator.TaraOrigine);
                cmd.Parameters.Add(producatorIdParam);
                cmd.Parameters.Add(numeProducatorParam);
                cmd.Parameters.Add(taraOrigineParam);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ProducatorExist(string producatorName)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("VerificaExistaNumeProducator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeProducatorParam = new SqlParameter("@nume_producator", producatorName);
                SqlParameter existaParam = new SqlParameter("@exista", SqlDbType.Bit);
                existaParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeProducatorParam);
                cmd.Parameters.Add(existaParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)existaParam.Value;
            }
        }


    }
}
