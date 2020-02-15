using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class StudentGradeDAL
    {
        public static void Insert(
            int studentID,
            int gradeID,
            decimal grade)
        {
            try
            {
                try
                {
                    Delete(studentID, gradeID);
                }
                catch { }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO student_grade VALUES ( " +
                    "{0},{1},{2});",
                    studentID, gradeID, grade);

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

        public static void Insert(
            int studentID,
            int gradeID)
        {
            try
            {
                try
                {
                    Delete(studentID, gradeID);
                }
                catch { }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO student_grade VALUES ( " +
                    "{0},{1},NULL);",
                    studentID, gradeID);

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

        public static void InsertIfNotExists(
            int studentID,
            int gradeID)
        {
            if(!StudentGradeExist(studentID,gradeID))
            {
                Insert(studentID,gradeID);
            }

        }

        public static void Delete(
            int student_id,
            int gradeID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM student_grade " +
                    "WHERE student_id = " + student_id + " " +
                        "AND grade_id = " + gradeID + ";";
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

        public static bool StudentGradeExist(int studentID, int gradeID)
        {
            try
            {
                DataTable dataTable = GetData(studentID, gradeID);
                return dataTable.Rows.Count == 1;
            }
            catch
            {
                return false;
            }

            
        }

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT student_grade.*, " +
                    "CONCAT(first_name,' ',last_name) AS student_name, " +
                    "grade_name "+
                "FROM student_grade sg" +
                    "JOIN users ON student_id = user_id " +
                    "JOIN grades ON grades.grade_id = student_grade.grade_id;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByGradeID(int gradeID)
        {
            string sqlString =
                "SELECT student_grade.*, " +
                    "CONCAT(first_name,' ',last_name) AS student_name, " +
                    "grade_name, " +
                    "( " +
                        "SELECT AVG(grade) " +
                        "FROM student_grade " +
                        "GROUP BY grade_id " +
                        "HAVING grade_id = " + gradeID + " " +
                    ") AS class_average " +
                "FROM student_grade " +
                    "JOIN users ON student_id = user_id " +
                    "JOIN grades ON grades.grade_id = student_grade.grade_id "+
                "WHERE student_grade.grade_id = " + gradeID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int studentID,int gradeID)
        {
            string sqlString =
                "SELECT student_grade.*, " +
                    "CONCAT(first_name,' ',last_name) AS student_name, " +
                    "grade_name, " +
                    "( "+
                        "SELECT AVG(grade) " +
                        "FROM student_grade " +
                        "GROUP BY grade_id "+
                        "HAVING grade_id = "+gradeID+" "+
                    ") AS class_average "+
                "FROM student_grade " +
                    "JOIN users ON student_id = user_id " +
                    "JOIN grades ON grades.grade_id = student_grade.grade_id "+
                "WHERE student_grade.grade_id = " + gradeID + " " +
                    "AND student_id = " + studentID + ";";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByStudentAndSection(int studentID, int sectionID)
        {
            string sqlString =
                "SELECT student_grade.*, "+
                    "grade_name, "+
                    "( " +
                        "SELECT AVG(grade) " +
                        "FROM student_grade sub_sg " +
                        "GROUP BY grade_id " +
                        "HAVING sub_sg.grade_id =  student_grade.grade_id " +
                    ") AS class_average " +
                "FROM student_grade " +
                    "JOIN grades ON student_grade.grade_id=grades.grade_id " +
                    "JOIN course_section ON grades.section_id = course_section.section_id " +
                    "JOIN course ON course_section.course_id = course.course_id "+
                "WHERE student_grade.student_id = " + studentID + " " +
                    "AND course_section.section_id = "+sectionID+";";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
