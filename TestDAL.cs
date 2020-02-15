using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBM_IncClasses;

namespace JBM_IncDAL
{
    public static class TestDAL
    {
        public static void Insert(
            string testName,
            int sectionID,
            bool enabled)
        {
            try
            {

                SQL_Escape.Escape(ref testName);
                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO tests VALUES ( " +
                    "'{0}',{1},{2});",
                    testName,sectionID,Converter.BoolToInt(enabled));

                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                DAL_Connection.Connection.Close();
            }

        }

        public static void Update(
            int testID,
            string testName,
            bool enabled)
        {

            int rowsUpdated = 0;
            try
            {

                JBM_IncClasses.SQL_Escape.Escape(ref testName);

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE tests SET " +
                        "name = '"+testName+"', "+
                        "enabled = "+Converter.BoolToInt(enabled)+" "+
                    "WHERE test_id = " + testID + ";";
                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                rowsUpdated = command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                DAL_Connection.Connection.Close();
            }

            if (rowsUpdated == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }
        }

        public static void Delete(int testID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM question_answers " +
                    "WHERE answer_id IN ( " +
                        "SELECT answer_id " +
                        "FROM question_answers " +
                            "JOIN test_question ON question_answers.question_id = test_question.question_id " +
                        "WHERE test_question.test_id = " + testID + "); " +

                    "DELETE FROM test_question " +
                    "WHERE test_id = " + testID + "; " +

                    "DELETE FROM tests " +
                    "WHERE test_id = " + testID + "; ";

                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                rowsDeleted = command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                DAL_Connection.Connection.Close();
            }

            if (rowsDeleted == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }
        }

        public static int LastTestID()
        {
            try
            {
                string sqlString =
                    "SELECT MAX(test_id) " +
                        "AS last_test_id " +
                        "FROM tests ;";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return int.Parse(dataTable.Rows[0]["last_test_id"].ToString());

            }
            catch
            {
                return -1;
            }
        }

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT * " +
                "FROM tests ";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
        public static DataTable GetData(int testID)
        {
            string sqlString =
                "SELECT * " +
                "FROM tests " +
                "WHERE test_id = " + testID + ";";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataBySection(int sectionID)
        {
            string sqlString =
                "SELECT * " +
                "FROM tests " +
                "WHERE section_id = " + sectionID + ";";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetEnabledTestBySection(int sectionID)
        {
            string sqlString =
                "SELECT * " +
                "FROM tests " +
                "WHERE section_id = " + sectionID + " " +
                    "AND enabled = 1;";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
