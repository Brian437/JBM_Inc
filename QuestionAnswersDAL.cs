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
    public static class QuestionAnswersDAL
    {
        public static void Insert(
            string answerValue,
            int questionID,
            bool rigthAnswer)
        {
            try
            {
                DAL_Connection.Connection.Open();

                string sqlString = string.Format(
                    "INSERT INTO question_answers VALUES ( " +
                    "'{0}',{1},{2} );",
                    answerValue, questionID,Converter.BoolToInt(rigthAnswer));

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
            int answerID,
            string answerValue,
            bool rightAnswer)

        {

            int rowsUpdated = 0;
            try
            {

                SQL_Escape.Escape(ref answerValue);

                DAL_Connection.Connection.Open();
                string sqlString =
                    "UPDATE question_answers SET " +
                        "answer_value = '"+answerValue +"', "+
                        "right_answer = "+Converter.BoolToInt(rightAnswer)+" "+
                    "WHERE answer_id = " + answerID + ";";
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

        public static void Delete(int answerID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM question_answers " +
                    "WHERE answer_id = " + answerID + ";";
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

        public static int LastAnswerID()
        {
            try
            {
                string sqlString =
                    "SELECT MAX(answer_id) " +
                        "AS last_answer_id " +
                    "FROM question_answers ;";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return int.Parse(dataTable.Rows[0]["last_answer_id"].ToString());

            }
            catch
            {
                return -1;
            }
        }

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT question_answers.*, " +
                    "test_question.question, "+
                    "tests.name "+
                "FROM question_answers " +
                    "JOIN test_question ON question_answers.question_id = test_question.question_id " +
                    "JOIN tests ON test_question.test_id = tests.test_id ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int answerID)
        {
            string sqlString =
                "SELECT question_answers.*, " +
                    "test_question.question, " +
                    "tests.name " +
                "FROM question_answers " +
                    "JOIN test_question ON question_answers.question_id = test_question.question_id " +
                    "JOIN tests ON test_question.test_id = tests.test_id " +
                "WHERE answer_id = " + answerID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDatabyQuestionID(int questionID)
        {
            string sqlString =
                "SELECT question_answers.*, " +
                    "test_question.question, " +
                    "tests.name " +
                "FROM question_answers " +
                    "JOIN test_question ON question_answers.question_id = test_question.question_id " +
                    "JOIN tests ON test_question.test_id = tests.test_id " +
                "WHERE question_answers.question_id = " + questionID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetRightAnswer(int questionID)
        {
            string sqlString =
                "SELECT question_answers.*, " +
                    "test_question.question, " +
                    "tests.name " +
                "FROM question_answers " +
                    "JOIN test_question ON question_answers.question_id = test_question.question_id " +
                    "JOIN tests ON test_question.test_id = tests.test_id " +
                "WHERE question_answers.question_id = " + questionID + " " +
                    "AND right_answer = 1; ";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
