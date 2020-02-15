using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class StudentQuizQuestionDAL
    {
        public static void Insert(
            int studentID,
            int questionID,
            int answerID)
        {
            try
            {
                try
                {
                    Delete(studentID, questionID);
                }
                catch { }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO student_quiz_question VALUES ( " +
                    "{0},{1},{2});",
                    studentID, questionID, answerID);

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
            int questionID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM student_quiz_question " +
                    "WHERE student_id = " + studentID + " " +
                        "AND question_id = " + questionID + ";";
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
                "SELECT student_quiz_question.*, " +
                    "CONCAT(first_name, ' ',last_name) AS full_name, " +
                    "test_question.question, " +
                    "selected_answer.answer_value AS answer_Selected, " +
                    "right_answer_table.answer_value AS the_right_answer "+
                "FROM student_quiz_question " +
                    "JOIN users ON user_id = student_id " +
                    "JOIN test_question ON student_quiz_question.question_id " +
                        "= test_question.question_id " +
                    "LEFT JOIN question_answers selected_answer " +
                        "ON student_quiz_question.answer_id = selected_answer.answer_id " +
                    "LEFT JOIN question_answers right_answer_table " +
                        "ON right_answer_table.question_id = test_question.question_id " +
                            "AND right_answer_table.right_answer = 1;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByTestID_AndStudentID(int testID,int studentID)
        {
            string sqlString =
                "SELECT student_quiz_question.*, " +
                    "CONCAT(first_name, ' ',last_name) AS full_name, " +
                    "test_question.question, " +
                    "selected_answer.answer_value AS answer_Selected, " +
                    "right_answer_table.answer_value AS the_right_answer " +
                "FROM student_quiz_question " +
                    "JOIN users ON user_id = student_id " +
                    "JOIN test_question ON student_quiz_question.question_id " +
                        "= test_question.question_id " +
                    "LEFT JOIN question_answers selected_answer " +
                        "ON student_quiz_question.answer_id = selected_answer.answer_id " +
                    "LEFT JOIN question_answers right_answer_table " +
                        "ON right_answer_table.question_id = test_question.question_id " +
                            "AND right_answer_table.right_answer = 1 "+
                "WHERE student_id = "+studentID +" "+
                    "AND test_id = "+testID+";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static int GetSelectedAnswerID(int studentID, int questionID)
        {
            try
            {
                string sqlString =
                    "SELECT * " +
                    "FROM student_quiz_question " +
                    "WHERE student_id = " + studentID + " " +
                        "AND question_id = " + questionID + ";";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return int.Parse(dataTable.Rows[0]["answer_id"].ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static bool AnswerIsRight(int studentID, int questionID)
        {
            string sqlString =
                "SELECT student_quiz_question.answer_id AS selected_answer, " +
                    "question_answers.answer_id AS correct_answer " +
                "FROM student_quiz_question " +
                    "JOIN question_answers ON student_quiz_question.question_id = question_answers.question_id " +
                "WHERE student_id = " + studentID + " " +
                    "AND student_quiz_question.question_id = " + questionID + " " +
                    "AND right_answer=1;";
                //"SELECT student_quiz_question.answer_id AS selected_answer " +
                //"FROM student_quiz_question ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
            {
                throw new KeyNotFoundException();
            }

            return dataTable.Rows[0]["selected_answer"].ToString() ==
                dataTable.Rows[0]["correct_answer"].ToString();

        }
    }
}
