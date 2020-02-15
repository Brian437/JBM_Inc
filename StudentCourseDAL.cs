using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class StudentCourseDAL
    {
        public static void Insert(
            int studentID,
            int sectionID)
        {
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO student_course VALUES " +
                    "({0},{1});",
                    studentID, sectionID);

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

        public static void Delete(
            int studentID,
            int sectionID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM student_course " +
                    "WHERE user_id = " + studentID + " " +
                        "AND section_id = " + sectionID + ";";
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

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT student_course.*, " +
                    "CONCAT(first_name,' ',last_name) as student_name, " +
                    "course_section.course_id, " +
                    "course_name "+
                "FROM student_course " +
                    "JOIN users " +
                        "ON student_course.user_id = users.user_id " +
                    "JOIN course_section " +
                        "ON course_section.section_id = student_course.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetStudents(int sectionID)
        {
            string sqlString =
                "SELECT student_course.*, " +
                    "CONCAT(first_name,' ',last_name) as student_name, " +
                    "course_section.course_id, " +
                    "course_name " +
                "FROM student_course " +
                    "JOIN users " +
                        "ON student_course.user_id = users.user_id " +
                    "JOIN course_section " +
                        "ON course_section.section_id = student_course.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id " +
                "WHERE student_course.section_id = " + sectionID+";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetCourses(int studentID)
        {
            string sqlString =
                "SELECT student_course.*, " +
                    "CONCAT(first_name,' ',last_name) as student_name, " +
                    "course_section.course_id, " +
                    "course_name " +
                "FROM student_course " +
                    "JOIN users " +
                        "ON student_course.user_id = users.user_id " +
                    "JOIN course_section " +
                        "ON course_section.section_id = student_course.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id "+
                "WHERE student_course.student_id = "+studentID+";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}

