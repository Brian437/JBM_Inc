/*
 * Coded by Brian Chaves
 * March 31,2014
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class SemesterLookupDAL
    {
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT * " +
                "FROM semester_lookup;";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            return dataTable;
        }

        public static string GetSemester(int semesterID)
        {
            try
            {
                string sqlString =
                    "SELECT semester_name " +
                    "FROM semester_lookup " +
                    "WHERE semester_id = " + semesterID + ";";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                return dataTable.Rows[0]["semester_name"].ToString();
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
