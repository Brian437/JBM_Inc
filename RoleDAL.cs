/*
 * Coded by Brian Chaves
 * March 31,2014
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBM_IncClasses;

namespace JBM_IncDAL
{
    public static class RoleDAL
    {
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT * " +
                "FROM user_role;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int roleID)
        {
            string sqlString =
                "SELECT * " +
                "FROM user_role " +
                "WHERE role_id = " + roleID + ";";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }

            return dataTable;
        }

        public static string GetRoleType(int roleID)
        {
            try
            {
                DataTable dataTable = GetData(roleID);
                return dataTable.Rows[0]["role_type"].ToString();
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
