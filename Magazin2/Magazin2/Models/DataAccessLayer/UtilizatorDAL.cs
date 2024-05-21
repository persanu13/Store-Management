using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magazin2.Services;
using System.Security.Policy;
using System.Windows;
using Magazin2.Models.EntityLayer;

namespace Magazin2.Models.DataAccessLayer
{
    public class UtilizatorDAL
    {
        public ObservableCollection<Utilizator> GetAllUtilizatori()
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectUtilizatori", con);
                ObservableCollection<Utilizator> result = new ObservableCollection<Utilizator>();
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Utilizator utilizator = new Utilizator();
                    utilizator.UtilizatorID = (int)(reader[0]);
                    utilizator.NumeUtilizator = reader.GetString(1);
                    utilizator.Parola = reader.GetString(2);
                    utilizator.TipUtilizator = reader.GetString(3);
                    result.Add(utilizator);
                }
                reader.Close();
                return result;
            }
        }
        public ObservableCollection<Utilizator> SearchUtilizatori(string sir,string type)
        {
            using (SqlConnection con = DbService.Connection)
            {
                ObservableCollection<Utilizator> result = new ObservableCollection<Utilizator>();
                SqlCommand cmd = new SqlCommand("FiltreazaUtilizatori", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sirParam = new SqlParameter("@sir_caractere", sir);
                SqlParameter tipUtilizatorParam = new SqlParameter();
                tipUtilizatorParam.ParameterName = "@tip_utilizator";
                if (string.IsNullOrEmpty(type))
                {
                    tipUtilizatorParam.Value = DBNull.Value;
                }
                else
                {
                    tipUtilizatorParam.Value = type;
                }
                cmd.Parameters.Add(sirParam);
                cmd.Parameters.Add(tipUtilizatorParam);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Utilizator utilizator = new Utilizator();
                    utilizator.UtilizatorID = (int)(reader[0]);
                    utilizator.NumeUtilizator = reader.GetString(1);
                    utilizator.Parola = reader.GetString(2);
                    utilizator.TipUtilizator = reader.GetString(3);
                    result.Add(utilizator);
                }
                reader.Close();
                return result;
            }
        }

        public void AddUtilizator(Utilizator utilizator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("AdaugaUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeUtilizatorParam = new SqlParameter("@nume_utilizator", utilizator.NumeUtilizator);
                SqlParameter parolaParam = new SqlParameter("@parola", utilizator.Parola);
                SqlParameter tipUtilizatorParam = new SqlParameter("@tip_utilizator", utilizator.TipUtilizator);
                SqlParameter utilizatorIdParam = new SqlParameter("@utilizator_id", SqlDbType.Int);
                utilizatorIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeUtilizatorParam);
                cmd.Parameters.Add(parolaParam);
                cmd.Parameters.Add(tipUtilizatorParam);
                cmd.Parameters.Add(utilizatorIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
                utilizator.UtilizatorID = utilizatorIdParam.Value as int?;
            }
        }
        public void DeleteUtilizator(Utilizator utilizator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("StergereLogicaUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter utilizatorIdParam = new SqlParameter("@utilizator_id", utilizator.UtilizatorID);
                cmd.Parameters.Add(utilizatorIdParam);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateUtilizator(Utilizator utilizator)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("ActualizeazaUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter utilizatorIdParam = new SqlParameter("@utilizator_id", utilizator.UtilizatorID);
                SqlParameter numeUtilizatorParam = new SqlParameter("@nume_utilizator", utilizator.NumeUtilizator);
                SqlParameter parolaParam = new SqlParameter("@parola", utilizator.Parola);
                SqlParameter tipUtilizatorParam = new SqlParameter("@tip_utilizator", utilizator.TipUtilizator);
                cmd.Parameters.Add(utilizatorIdParam);
                cmd.Parameters.Add(numeUtilizatorParam);
                cmd.Parameters.Add(parolaParam);
                cmd.Parameters.Add(tipUtilizatorParam);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool UserNameExist(string userName)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("VerificaExistaNumeUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter numeUtilizatorParam = new SqlParameter("@nume_utilizator", userName);
                SqlParameter existaParam = new SqlParameter("@exista", SqlDbType.Bit);
                existaParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(numeUtilizatorParam);
                cmd.Parameters.Add(existaParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)existaParam.Value;
            }
        }

        public bool UserPasswordExist(string password)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("VerificaExistaParolaUtilizator", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parolaParam = new SqlParameter("@parola", password);
                SqlParameter existaParam = new SqlParameter("@exista", SqlDbType.Bit);
                existaParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parolaParam);
                cmd.Parameters.Add(existaParam);
                con.Open();
                cmd.ExecuteNonQuery();
                return (bool)existaParam.Value;
            }
        }

        public ObservableCollection<Tuple<int, double>> TotalMoneyPerDay(int? utilizatorId,DateTime? date)
        {
            using (SqlConnection con = DbService.Connection)
            {
                SqlCommand cmd = new SqlCommand("SelectSumaTotalaZilnicaByUtilizator", con);
                ObservableCollection<Tuple<int, double>> result = new ObservableCollection<Tuple<int, double>>();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@utilizator_id", utilizatorId);
                cmd.Parameters.AddWithValue("@data_calendaristica", date);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                int day = 0;
                while (reader.Read())
                {
                    day++;
                    Tuple<int, double> touple = new Tuple<int, double>(day, (double)reader[1]);
                    result.Add(touple);
                }
                reader.Close();
                return result;
            }
        }




    }
}
