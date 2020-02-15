using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class RoomTypeDAL
    {
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT * " +
                "FROM room_type_lookup;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static string GetRoomType(int roomTypeID)
        {
            try
            {
                string sqlString =
                    "SELECT room_type_name " +
                    "FROM room_type_lookup " +
                    "WHERE room_type_id = " + roomTypeID + ";";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                return dataTable.Rows[0]["room_type_name"].ToString();
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
