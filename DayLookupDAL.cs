using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class DayLookupDAL
    {
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT * " +
                "FROM day_lookup;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static string GetDay(int dayID)
        {
            try
            {
                string sqlString =
                    "SELECT day_name " +
                    "FROM day_lookup " +
                    "WHERE day_id = " + dayID + ";";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable.Rows[0]["day_name"].ToString();
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
