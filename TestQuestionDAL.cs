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
    public static class TestQuestionDAL
    {
        public static void Insert(
            int testID,
            string question)
        {
            try
            {
                DAL_Connection.Connection.Open();

                string sqlString = string.Format(
                    "INSERT INTO test_question VALUES ( " +
                    "{0},'{1}' );",
                    testID,question);

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
            int questionID,
            string question)
        {

            int rowsUpdated = 0;
            try
            {
                DAL_Connection.Connection.Open();

                SQL_Escape.Escape(ref question);

                string sqlString =
                    "UPDATE test_question SET " +
                        "question = '"+question +"' " +
                    "WHERE question_id = " + questionID + ";";

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

        public static void Delete(int questionID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM question_answers "+
                    "WHERE question_id = " +questionID+";"+

                    "DELETE FROM test_question " +
                    "WHERE question_id = " + questionID + ";";
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

        public static int LastTestID()
        {
            try
            {
                string sqlString =
                    "SELECT MAX(question_id) " +
                        "AS last_question_id " +
                        "FROM test_question ;";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return int.Parse(dataTable.Rows[0]["last_question_id"].ToString());

            }
            catch
            {
                return -1;
            }
        }

        public static DataTable GetData()
        {
            string sqlString =
                "SELECT test_question.*, " +
                    "tests.name " +
                "FROM test_question " +
                    "JOIN tests ON test_question.test_id = tests.test_id ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetData(int questionID)
        {
            string sqlString =
                "SELECT test_question.*, " +
                    "tests.name " +
                "FROM test_question " +
                    "JOIN tests ON test_question.test_id = tests.test_id " +
                "WHERE question_id = " + questionID + " ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static DataTable GetDataByTestID(int testID)
        {
            string sqlString =
                "SELECT test_question.*, " +
                    "tests.name " +
                "FROM test_question " +
                    "JOIN tests ON test_question.test_id = tests.test_id " +
                "WHERE test_question.test_id = " + testID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static int RightAnswerCount(int questionID)
        {
            DataTable answerTable = new DataTable();
            int rightAnswerCount = 0;
            try
            {
                answerTable = QuestionAnswersDAL.GetDatabyQuestionID(questionID);
                for (int x = 0; x < answerTable.Rows.Count; x++)
                {
                    string test = answerTable.Rows[x]["right_answer"].ToString();
                    if (answerTable.Rows[x]["right_answer"].ToString() == "True")
                    {
                        rightAnswerCount++;
                    }
                }
                return rightAnswerCount;
            }
            catch
            {
                return -1;
            }
        }

        public static int AnswerCount(int questionID)
        {
            DataTable answerTable = new DataTable();
            try
            {
                answerTable = QuestionAnswersDAL.GetDatabyQuestionID(questionID);
                return answerTable.Rows.Count;
            }
            catch
            {
                return -1;
            }
        }
    }
}
