/*
 * Brian Chaves
 * April 21,2014
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JBM_IncClasses;

namespace JBM_IncDAL
{
    public static class ClassTimeDAL
    {
        /// <summary>
        /// Inserts to the database
        /// </summary>
        /// <param name="dayID">day ID</param>
        /// <param name="startTime">start time in 24 hour format</param>
        /// <param name="endTime">end time in 24 hour format</param>
        /// <param name="sectionID">section ID</param>
        /// <param name="classRoomNumber">class room number</param>
        public static void Insert(
            int dayID,
            decimal startTime,
            decimal endTime,
            int sectionID,
            string classRoomNumber)
        {
            try
            {
                SQL_Escape.Escape(ref classRoomNumber);

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO class_time VALUES " +
                    "({0},{1},{2},{3},'{4}');",
                    dayID, startTime, endTime, sectionID, classRoomNumber);

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
        /// updates the database
        /// </summary>
        /// <param name="classTimeID">Class Time ID</param>
        /// <param name="dayID">day ID</param>
        /// <param name="startTime">start time in 24 hour format</param>
        /// <param name="endTime">end time in 24 hour format</param>
        /// <param name="sectionID">section ID</param>
        /// <param name="classRoomNumber">class room number</param>
        public static void Update(
            int classTimeID,
            int dayID,
            decimal startTime,
            decimal endTime,
            int sectionID,
            string classRoomNumber)
        {
            int rowsUpdated = 0;
            try
            {
                SQL_Escape.Escape(ref classRoomNumber);

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE class_time SET " +
                        "day = " + dayID + ", " +
                        "start_time = " + startTime + ", " +
                        "end_time = " + endTime + ", " +
                        "section_id = " + sectionID + ", " +
                        "class_Room_Number = '" + classRoomNumber + "' " +
                    "WHERE class_time_id = " + classTimeID;
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
        /// Deletes from the database
        /// </summary>
        /// <param name="classTimeID">Class time ID</param>
        public static void Delete(int classTimeID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM class_time " +
                    "WHERE class_time_id = " + classTimeID + ";";
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
                "SELECT class_time.*, " +
                    "day_name, " +
                    "course_section.course_id, " +
                    "course_name, " +
                    "room_type_name, " +
                    "CONCAT (first_name,' ',last_name) AS teacher_name, " +
                    "year, " +
                    "semester_name, " +
                    "CONCAT(semester_name,' ',year) AS semester_and_year " +
                "FROM class_time " +
                    "LEFT JOIN day_lookup ON day_id = day " +
                    "LEFT JOIN course_section ON course_section.section_id = class_time.section_id " +
                    "LEFT JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN class_room ON class_time.class_room_number = class_room.class_room_number " +
                    "LEFT JOIN room_type_lookup ON room_type=room_type_id " +
                    "LEFT JOIN users teacher ON teacher_id = user_id " +
                    "LEFT JOIN semester_lookup ON course_section.semester_id = semester_lookup.semester_id";


            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GetExtraRows(dataTable);

            return dataTable;
        }

        /// <summary>
        /// Gets one row
        /// </summary>
        /// <param name="classTimeID">class time ID</param>
        /// <returns>one row</returns>
        public static DataTable GetData(int classTimeID)
        {

            string sqlString =
                "SELECT class_time.*, " +
                    "day_name, " +
                    "course_section.course_id, " +
                    "course_name, " +
                    "room_type_name, " +
                    "CONCAT (first_name,' ',last_name) AS teacher_name, " +
                    "year, " +
                    "semester_name, " +
                    "CONCAT(semester_name,' ',year) AS semester_and_year, " +
                    "course_descripton " +
                "FROM class_time " +
                    "LEFT JOIN day_lookup ON day_id = day " +
                    "LEFT JOIN course_section ON course_section.section_id = class_time.section_id " +
                    "LEFT JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN class_room ON class_time.class_room_number = class_room.class_room_number " +
                    "LEFT JOIN room_type_lookup ON room_type=room_type_id " +
                    "LEFT JOIN users teacher ON teacher_id = user_id " +
                    "LEFT JOIN semester_lookup ON course_section.semester_id = semester_lookup.semester_id " +
                "WHERE class_time_id = " + classTimeID;



            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GetExtraRows(dataTable);

            return dataTable;
        }

        /// <summary>
        /// Gets data
        /// Filtered by student ID
        /// </summary>
        /// <param name="studentID">student ID</param>
        /// <returns>data</returns>
        public static DataTable GetDataFromStudentID(int studentID)
        {
            string sqlString =
                "SELECT class_time.*, " +
                    "day_name, " +
                    "course_section.course_id, " +
                    "course_name, " +
                    "room_type_name, " +
                    "CONCAT (teacher.first_name,' ',teacher.last_name) AS teacher_name, " +
                    "year, " +
                    "semester_name, " +
                    "CONCAT(semester_name,' ',year) AS semester_and_year " +
                "FROM class_time " +
                    "LEFT JOIN day_lookup ON day_id = day " +
                    "LEFT JOIN course_section ON course_section.section_id = class_time.section_id " +
                    "LEFT JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN class_room ON class_time.class_room_number = class_room.class_room_number " +
                    "LEFT JOIN room_type_lookup ON room_type=room_type_id " +
                    "LEFT JOIN users teacher ON teacher_id = user_id " +
                    "LEFT JOIN semester_lookup ON course_section.semester_id = semester_lookup.semester_id " +
                    "LEFT JOIN student_course ON student_course.section_id = class_time.section_id " +
                    "LEFT JOIN users students ON student_course.user_id = students.user_id " +
                "WHERE students.user_id = " + studentID + " " +
                "ORDER BY day ASC, start_time ASC ;";



            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GetExtraRows(dataTable);

            return dataTable;
        }

        /// <summary>
        /// Get data
        /// Filtered by section
        /// </summary>
        /// <param name="sectionID">section ID</param>
        /// <returns>Data</returns>
        public static DataTable GetDataBySectionID(int sectionID)
        {
            string sqlString =
                "SELECT class_time.*, " +
                    "day_name, " +
                    "course_section.course_id, " +
                    "course_name, " +
                    "room_type_name, " +
                    "CONCAT (teacher.first_name,' ',teacher.last_name) AS teacher_name, " +
                    "year, " +
                    "semester_name, " +
                    "CONCAT(semester_name,' ',year) AS semester_and_year " +
                "FROM class_time " +
                    "LEFT JOIN day_lookup ON day_id = day " +
                    "LEFT JOIN course_section ON course_section.section_id = class_time.section_id " +
                    "LEFT JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN class_room ON class_time.class_room_number = class_room.class_room_number " +
                    "LEFT JOIN room_type_lookup ON room_type=room_type_id " +
                    "LEFT JOIN users teacher ON teacher_id = user_id " +
                    "LEFT JOIN semester_lookup ON course_section.semester_id = semester_lookup.semester_id " +
                "WHERE class_time.section_id = " + sectionID;



            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GetExtraRows(dataTable);

            return dataTable;
        }

        public static DataTable GetDataByTeacherID(int teacherID)
        {
            string sqlString =
                "SELECT class_time.*, " +
                    "day_name, " +
                    "course_section.course_id, " +
                    "course_name, " +
                    "room_type_name, " +
                    "CONCAT (teacher.first_name,' ',teacher.last_name) AS teacher_name, " +
                    "year, " +
                    "semester_name, " +
                    "CONCAT(semester_name,' ',year) AS semester_and_year " +
                "FROM class_time " +
                    "LEFT JOIN day_lookup ON day_id = day " +
                    "LEFT JOIN course_section ON course_section.section_id = class_time.section_id " +
                    "LEFT JOIN course ON course_section.course_id = course.course_id " +
                    "LEFT JOIN class_room ON class_time.class_room_number = class_room.class_room_number " +
                    "LEFT JOIN room_type_lookup ON room_type=room_type_id " +
                    "LEFT JOIN users teacher ON teacher_id = user_id " +
                    "LEFT JOIN semester_lookup ON course_section.semester_id = semester_lookup.semester_id " +
                "WHERE teacher.user_id = " + teacherID + " " +
                "ORDER BY day ASC, start_time ASC ;";
                //"SELECT * FROM class_Time";



            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            GetExtraRows(dataTable);

            return dataTable;
        }

        /// <summary>
        /// Puts the extra rows in the database
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        private static void GetExtraRows(DataTable dataTable)
        {
            dataTable.Columns.Add("start_time_12hour");
            dataTable.Columns.Add("end_time_12hour");
            dataTable.Columns.Add("length");

            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                int startTime24Hour = int.Parse(dataTable.Rows[x]["start_time"].ToString());
                string startTime12Hour = Converter.Time24To12(startTime24Hour);
                dataTable.Rows[x]["start_time_12hour"] = startTime12Hour;
                int endTime24Hour = int.Parse(dataTable.Rows[x]["end_time"].ToString());
                string endTime12Hour = Converter.Time24To12(endTime24Hour);
                dataTable.Rows[x]["end_time_12hour"] = endTime12Hour;
                int length = endTime24Hour - startTime24Hour;
                dataTable.Rows[x]["length"] = length;
            }
        }
    }
}
