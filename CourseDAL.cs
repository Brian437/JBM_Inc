
/*
 * Coded by Brian Chaves
 * March 31,2014
 */
using JBM_IncClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class CourseDAL
    {
        /// <summary>
        /// Inserts data into database
        /// </summary>
        /// <param name="courseID">course number</param>
        /// <param name="courseName">course name</param>
        /// <param name="courseDescription">description</param>
        /// <param name="cost">cost</param>
        public static void Insert(
            string courseID,
            string courseName,
            string courseDescription,
            decimal cost)
        {
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref courseID);
                SQL_Escape.Escape(ref courseName);
                SQL_Escape.Escape(ref courseDescription);

                string sqlString = string.Format(
                    "INSERT INTO course VALUES ( " +
                    "'{0}','{1}','{2}',{3});",
                    courseID, courseName, courseDescription,cost);

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

        /// <summary>
        /// Updates the database
        /// </summary>
        /// <param name="courseID">course number</param>
        /// <param name="courseName">course name</param>
        /// <param name="courseDescription">description</param>
        /// <param name="cost">cost</param>
        public static void Update(
            string courseID,
            string courseName,
            string courseDescription,
            decimal cost)
        {

            int rowsUpdated = 0;
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref courseID);
                SQL_Escape.Escape(ref courseName);
                SQL_Escape.Escape(ref courseDescription);

                string sqlString =
                    "UPDATE course SET " +
                        "course_name = '" + courseName + "', " +
                        "course_descripton = '" + courseDescription + "', " +
                        "cost = "+cost+" "+
                    "WHERE course_id = '" + courseID + "';";
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

        /// <summary>
        /// Deletes from the database
        /// </summary>
        /// <param name="courseID">Course number</param>
        public static void Delete(string courseID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref courseID);

                string sqlString =
                    "DELETE FROM course " +
                    "WHERE course_id = '" + courseID + "';";
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

        /// <summary>
        /// Gets the data
        /// </summary>
        /// <returns>returns data</returns>
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT *, " +
                    "CONCAT(course_id,':',course_name) as id_and_name "+
                "FROM course;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Gets one row of data
        /// </summary>
        /// <param name="courseID">Course number</param>
        /// <returns>data</returns>
        public static DataTable GetData(string courseID)
        {

            SQL_Escape.Escape(ref courseID);

            string sqlString =
                "SELECT *, " +
                    "CONCAT(course_id,':',course_name) as id_and_name " +
                "FROM course " +
                "WHERE course_id ='" +courseID+"';";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }

            return dataTable;
        }

    }
}
