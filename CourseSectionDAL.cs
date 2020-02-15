using JBM_IncClasses;
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
    public static class CourseSectionDAL
    {
        public static void Insert(
            string courseID,
            int semesterID,
            int year,
            int teacherID,
            int forumAccessibilityID)
        {
            string strTeacherID;
            try
            {
                SQL_Escape.Escape(ref courseID);
                if (teacherID == 0)
                {
                    strTeacherID = "NULL";
                }
                else
                {
                    strTeacherID = teacherID.ToString();
                }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO course_section VALUES " +
                    "('{0}',{1},{2},{3},{4});",
                    courseID, semesterID, year, strTeacherID, forumAccessibilityID);

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
            int sectionID,
            string courseID,
            int semesterID,
            int year,
            int teacherID,
            int forumAccessibilityID)
        {

            int rowsUpdated = 0;
            string strTeacherID;
            try
            {
                SQL_Escape.Escape(ref courseID);
                if (teacherID == 0)
                {
                    strTeacherID = "NULL";
                }
                else
                {
                    strTeacherID = teacherID.ToString();
                }

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE course_section SET " +
                        "course_id = '"+courseID+"', " +
                        "semester_id = "+semesterID+" , "+
                        "year = " +year+", "+
                        "teacher_id = "+strTeacherID+"  "+
                    "WHERE section_id = " + sectionID + ";";
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

        public static void Delete(int sectionID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM course_section " +
                    "WHERE section_id = " + sectionID + ";";
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
                "SELECT course_section.* ," +
                    "course.course_Name , " +
                    "CONCAT(users.first_name,' ',users.last_name) AS teacher, " +
                    "semester_name, "+
                    "CONCAT(section_id,' - ',course.course_id) AS section_and_course_id, " +
                    "CONCAT (year,'-',semester_name) AS year_and_semester "+
                "FROM course_Section " +
                    "JOIN course " +
                        "ON course_section.course_id = course.course_id " +
                    "LEFT JOIN users " +
                        "ON teacher_id = user_id " +
                    "JOIN semester_lookup " +
                        "ON course_section.semester_id=semester_lookup.semester_id ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int sectionID)
        {
            string sqlString =
                "SELECT course_section.* ," +
                    "course.course_Name , " +
                    "CONCAT(users.first_name,' ',users.last_name) AS teacher, " +
                    "semester_name, " +
                    "CONCAT(section_id,' - ',course.course_id) AS section_and_course_id, " +
                    "CONCAT (year,'-',semester_name) AS year_and_semester " +
                "FROM course_Section " +
                    "JOIN course " +
                        "ON course_section.course_id = course.course_id " +
                    "LEFT JOIN users " +
                        "ON teacher_id = user_id " +
                    "JOIN semester_lookup " +
                        "ON course_section.semester_id=semester_lookup.semester_id " +
                "WHERE section_id = " + sectionID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByCourseID(string courseID)
        {
            SQL_Escape.Escape(ref courseID);

            string sqlString =
                "SELECT course_section.* ," +
                    "course.course_Name , " +
                    "CONCAT(users.first_name,' ',users.last_name) AS teacher, " +
                    "semester_name, " +
                    "CONCAT(section_id,' - ',course.course_id) AS section_and_course_id, " +
                    "CONCAT (year,'-',semester_name) AS year_and_semester " +
                "FROM course_Section " +
                    "JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN users ON teacher_id = user_id " +
                    "JOIN semester_lookup ON course_section.semester_id=semester_lookup.semester_id " +
                "WHERE course_section.course_id = '" + courseID + "';";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByTeacherID(int teacherID)
        {

            string sqlString =
                "SELECT course_section.* ," +
                    "course.course_Name , " +
                    "CONCAT(users.first_name,' ',users.last_name) AS teacher, " +
                    "semester_name, " +
                    "CONCAT(section_id,' - ',course.course_id) AS section_and_course_id, " +
                    "CONCAT (year,'-',semester_name) AS year_and_semester " +
                "FROM course_Section " +
                    "JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN users ON teacher_id = user_id " +
                    "JOIN semester_lookup ON course_section.semester_id=semester_lookup.semester_id " +
                "WHERE teacher_id = " + teacherID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int year,int semesterID)
        {
            string sqlString =
                "SELECT course_section.* ," +
                    "course.course_Name , " +
                    "CONCAT(users.first_name,' ',users.last_name) AS teacher, " +
                    "semester_name, " +
                    "CONCAT(section_id,' - ',course.course_id) AS section_and_course_id, " +
                    "CONCAT (year,'-',semester_name) AS year_and_semester " +
                "FROM course_Section " +
                    "JOIN course " +
                        "ON course_section.course_id = course.course_id " +
                    "LEFT JOIN users " +
                        "ON teacher_id = user_id " +
                    "JOIN semester_lookup " +
                        "ON course_section.semester_id=semester_lookup.semester_id " +
                "WHERE year = " + year + " " +
                    "AND semester_id = " + semesterID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
