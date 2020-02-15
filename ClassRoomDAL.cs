/*
 * Brian Chaves
 * April 21,2014
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
    public static class ClassRoomDAL
    {
        /// <summary>
        /// Inserts into datatable
        /// </summary>
        /// <param name="classRoomNumber">class room number</param>
        /// <param name="roomType">room type ID</param>
        public static void Insert(
            string classRoomNumber,
            int roomType)
        {
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref classRoomNumber);

                string sqlString = string.Format(
                    "INSERT INTO class_room VALUES ( " +
                    "'{0}',{1});",
                    classRoomNumber, roomType);

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
        /// <param name="classRoomNumber">class room number</param>
        /// <param name="roomTypeID">room type ID</param>
        public static void Update(
            string classRoomNumber,
            int roomTypeID)
        {

            int rowsUpdated = 0;
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref classRoomNumber);

                string sqlString =
                    "UPDATE class_room SET " +
                        "room_type = " + roomTypeID + " " +
                    "WHERE class_room_number = '" + classRoomNumber + "';";
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
        /// Deletes data
        /// </summary>
        /// <param name="classRoomNumber">class room number</param>
        public static void Delete(string classRoomNumber)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref classRoomNumber);

                string sqlString =
                    "DELETE FROM class_room " +
                    "WHERE class_room_number = '" + classRoomNumber + "';";
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
        /// Gets data
        /// </summary>
        /// <returns>data</returns>
        public static DataTable GetData()
        {
            string sqlString =
                "SELECT class_room.*, " +
                    "room_type_name "+
                "FROM class_room " +
                    "LEFT JOIN room_type_lookup ON room_type_id = room_type;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Gets data based on class room number
        /// </summary>
        /// <param name="classRoomNumber">class room number</param>
        /// <returns>data</returns>
        public static DataTable GetData(string classRoomNumber)
        {
            string sqlString =
                "SELECT class_room.*, " +
                    "room_type_name " +
                "FROM class_room " +
                    "LEFT JOIN room_type_lookup ON room_type_id = room_type " +
                "WHERE class_room_number = '" + classRoomNumber + "';";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
