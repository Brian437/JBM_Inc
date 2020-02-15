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
    public static class GradeDAL
    {
        public static void Insert(
            int sectionID,
            decimal weigth,
            int testID,
            string gradeName)
        {
            string strTestID = "";

            try
            {
                if (testID == 0)
                {
                    strTestID="NULL";
                }
                else
                {
                    strTestID=testID.ToString();
                }

                DAL_Connection.Connection.Open();
                SQL_Escape.Escape(ref gradeName);
                string sqlString = string.Format(
                    "INSERT INTO grades VALUES ( " +
                    "{0},{1},{2},'{3}');",
                    sectionID, weigth, strTestID, gradeName);
                    

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
            int gradeID,
            int sectionID,
            decimal weigth,
            int testID,
            string gradeName)
        {

            int rowsUpdated = 0;
            string strTestID = "";

            try
            {
                DAL_Connection.Connection.Open();
                SQL_Escape.Escape(ref gradeName);

                if (testID == 0)
                {
                    strTestID="NULL";
                }
                else
                {
                    strTestID = testID.ToString();
                }

                string sqlString =
                    "UPDATE grades SET " +
                        "section_id = "+sectionID+", "+
                        "weight = "+weigth+", "+
                        "test_id = "+strTestID+", "+
                        "grade_name = '"+gradeName +"' "+
                    "WHERE grade_id = " + gradeID + ";";
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

        public static void Delete(int gradeID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM student_grade "+
                    "WHERE grade_id = "+gradeID+"; "+

                    "DELETE FROM grades " +
                    "WHERE grade_id = " + gradeID + ";";
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
                "SELECT grades.*, course.course_id, course.course_name " +
                "FROM grades " +
                    "JOIN course_section " +
                        "ON grades.section_id = course_section.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataBySection(int sectionID)
        {
            string sqlString =
                "SELECT grades.*, course.course_id, course.course_name " +
                "FROM grades " +
                    "JOIN course_section " +
                        "ON grades.section_id = course_section.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id " +
                "WHERE grades.section_id = "+ +sectionID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int gradeID)
        {
            string sqlString =
                "SELECT grades.*, course.course_id, course.course_name " +
                "FROM grades " +
                    "JOIN course_section " +
                        "ON grades.section_id = course_section.section_id " +
                    "JOIN course " +
                        "ON course.course_id = course_section.course_id " +
                "WHERE grade_id= " + gradeID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static int GetSectionID(int gradeID)
        {
            DataTable gradeData;
            gradeData = GetData(gradeID);
            return int.Parse(gradeData.Rows[0]["section_id"].ToString());
        }

        public static int GetSectionID(string gradeID)
        {
            return GetSectionID(int.Parse(gradeID));
        }

        public static decimal GetClassAverage(int gradeID)
        {
            DataTable dataTable = StudentGradeDAL.GetDataByGradeID(gradeID);
            int gradeCount = 0;
            decimal totalValue = 0;

            if(dataTable.Rows.Count==0)
            {
                return 0;
            }

            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                decimal grade = decimal.Parse(dataTable.Rows[x]["grade"].ToString());
                gradeCount++;
                totalValue += grade;
            }

            return (totalValue/(decimal)gradeCount);

        }
    }
}
