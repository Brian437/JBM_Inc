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
    public static class UserDAL
    {

        public static void Insert(
            int roleID,
            string userName,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string city,
            string province,
            string country,
            string password)
        {
            string hashPassword="";
            string passwordSalt = "";
            try
            {
                SQL_Escape.Escape(ref userName);
                SQL_Escape.Escape(ref firstName);
                SQL_Escape.Escape(ref lastName);
                SQL_Escape.Escape(ref phoneNumber);
                SQL_Escape.Escape(ref email);
                SQL_Escape.Escape(ref city);
                SQL_Escape.Escape(ref province);
                SQL_Escape.Escape(ref country);

                if (password == "")
                {
                    hashPassword = "NULL";
                    passwordSalt = "NULL";
                }
                else
                {
                    passwordSalt = Password.GenerateSalt(20, 50);
                    hashPassword = Password.CreateHash(password + passwordSalt);
                    SQL_Escape.Escape(ref hashPassword);
                    SQL_Escape.Escape(ref passwordSalt);
                    hashPassword = "'" + hashPassword + "'";
                    passwordSalt = "'" + passwordSalt + "'";
                }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO users VALUES ( " +
                    "{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9}, {10} );",
                    roleID, userName, firstName, lastName, phoneNumber, email,
                    city, province, country, hashPassword,passwordSalt);

                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                command.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                throw exception;
            }
            finally
            {
                DAL_Connection.Connection.Close();
            }

        }

        public static void Insert(User user)
        {
            Insert(user.RoleID, user.UserName, user.FirstName, user.LastName,
                user.PhoneNumber, user.Email, user.City, user.Province, user.Country,"");
        }

        public static void Update(
            int userID,
            int roleID,
            string userName,
            string firstName,
            string lastName,
            string phoneNumber,
            string email,
            string city,
            string province,
            string country)
        {
            
            int rowsUpdated = 0;
            try
            {

                SQL_Escape.Escape(ref userName);
                SQL_Escape.Escape(ref firstName);
                SQL_Escape.Escape(ref lastName);
                SQL_Escape.Escape(ref phoneNumber);
                SQL_Escape.Escape(ref email);
                SQL_Escape.Escape(ref city);
                SQL_Escape.Escape(ref province);
                SQL_Escape.Escape(ref country);

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE users SET " +
                        "role = " + roleID + ", " +
                        "user_name = '" + userName + "', " +
                        "first_name = '" + firstName + "', " +
                        "last_name = '" + lastName + "', " +
                        "phone_number = '" + phoneNumber + "', " +
                        "email = '" + email + "', " +
                        "city = '" + city + "', " +
                        "province = '" + province + "', " +
                        "country = '" + country + "' " +
                    "WHERE user_id = " + userID +";";
                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                rowsUpdated = command.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                throw exception;
            }
            finally
            {
                DAL_Connection.Connection.Close();
            }

            if(rowsUpdated==0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }
        }

        public static void SetPassword(
            int userID,
            string password)
        {

            int rowsUpdated = 0;
            string hashPassword = "";
            string passwordSalt="";

            try
            {
                SQL_Escape.Escape(ref hashPassword);
                passwordSalt = Password.GenerateSalt(20, 50);
                hashPassword = Password.CreateHash(password+passwordSalt);

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE users SET " +
                        "password = '" + hashPassword + "', " +
                        "password_salt = '"+passwordSalt+"' "+
                    "WHERE user_id = " + userID + ";";
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

        //public static void SetPassword(
        //    string userName,
        //    string password)
        //{

        //    int rowsUpdated = 0;
        //    string hashPassword = "";

        //    try
        //    {
        //        hashPassword=Password.CreateHash(password);
        //        SQL_Escape.EscapeString(ref hashPassword);

        //        DAL_Connection.Connection.Open();
        //        string sqlString =
        //            "UPDATE users SET " +
        //                "password = '" + hashPassword + "' " +
        //            "WHERE user_name = '" + userName + "';";
        //        SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
        //        rowsUpdated = command.ExecuteNonQuery();
        //    }
        //    catch (Exception exception)
        //    {
        //        throw exception;
        //    }
        //    finally
        //    {
        //        DAL_Connection.Connection.Close();
        //    }

        //    if (rowsUpdated == 0)
        //    {
        //        throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
        //    }
        //}

        public static void Update(User user)
        {
            Update(user.UserID, user.RoleID, user.UserName, user.FirstName, user.LastName,
                user.PhoneNumber, user.Email, user.City,
                user.Province, user.Country);
        }

        public static void Delete(int userID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM users " +
                    "WHERE user_id = " + userID +";";
                SqlCommand command = new SqlCommand(sqlString, DAL_Connection.Connection);
                rowsDeleted= command.ExecuteNonQuery();
            }
            catch(Exception exception)
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

        public static void Delete(User user)
        {
            Delete(user.UserID);
        }

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT users.*, role_type, " +
                    "CONCAT(first_name,' ',last_name) AS full_name "+
                "FROM users " +
                    "JOIN user_role " +
                        "ON users.role = role_id;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }


        public static DataTable GetAdmin()
        {
            string sqlString =
                "SELECT users.*, " +
                    "CONCAT(first_name,' ',last_name) AS full_name " +
                "FROM users " +
                "WHERE role = 1;";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetTeacher()
        {
            string sqlString =
                "SELECT users.*, " +
                    "CONCAT(first_name,' ',last_name) AS full_name " +
                "FROM users " +
                "WHERE role = 2;";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetStudent()
        {
            string sqlString =
                "SELECT users.*, " +
                    "CONCAT(first_name,' ',last_name) AS full_name " +
                "FROM users " +
                "WHERE users.role = 3;";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int userID)
        {
            string sqlString =
                "SELECT users.*, role_type, " +
                    "CONCAT(first_name,' ',last_name) AS full_name " +
                "FROM users " +
                    "JOIN user_role " +
                        "ON users.role = role_id " +
                "WHERE user_id = '" + userID+"';";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }

            return dataTable;
        }

        public static DataTable GetData(string userName)
        {
            SQL_Escape.Escape(ref userName);

            string sqlString =
                "SELECT users.*, role_type, " +
                    "CONCAT(first_name,' ',last_name) AS full_name " +
                "FROM users " +
                    "JOIN user_role " +
                        "ON users.role = role_id " +
                "WHERE user_name = '" + userName + "';";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                throw new KeyNotFoundException(Shared.NOT_FOUND_STRING);
            }

            return dataTable;
        }

        public static int GetUserID(string userName)
        {
            try
            {
                DataTable userData = GetData(userName);
                int userID=int.Parse(userData.Rows[0]["user_id"].ToString());
                return userID;
            }
            catch
            {
                return -1;
            }
        }

        public static string GetUserName(int userID)
        {
            try
            {
                DataTable dataTable = GetData(userID);
                return dataTable.Rows[0]["user_name"].ToString();
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }

        public static User GetUser(int userID)
        {
            throw new NotImplementedException();
        }

    }
}
