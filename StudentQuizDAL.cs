using JBM_IncClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBM_IncDAL
{
    public static class StudentQuizDAL
    {
        public static void Insert(
            int studentID,
            int testID,
            bool completed)
        {
            try
            {
                try
                {
                    Delete(studentID, testID);
                }
                catch { }

                DAL_Connection.Connection.Open();
                string sqlString = string.Format(
                    "INSERT INTO student_quiz VALUES ( " +
                    "{0},{1},{2});",
                    studentID,testID,Converter.BoolToInt(completed));

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
            int testID)
        {
            int rowsDeleted = 0;
            try
            {
                DAL_Connection.Connection.Open();
                string sqlString =
                    "DELETE FROM student_quiz " +
                    "WHERE student_id = " + studentID + " " +
                        "AND test_id = " + testID + ";";
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
                "SELECT student_quiz.*, " +
                    "CONCAT(first_name, ' ',last_name) AS full_name, "+
                    "tests.name "+
                "FROM student_quiz " +
                    "JOIN users ON user_id = student_id " +
                    "JOIN tests ON student_quiz.test_id = tests.test_id ;";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);
            return dataTable;
        }

        public static bool TestEnabled(int studentID, int testID)
        {
            try
            {
                DataTable testData = TestDAL.GetData(testID);
                if (testData.Rows[0]["enabled"].ToString() != "True")
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            //return true;

            return !(TestCompleted(studentID,testID));
        }

        public static bool TestCompleted(int studentID, int testID)
        {
            try
            {
                string sqlString =
                "SELECT * " +
                "FROM student_quiz " +
                "WHERE student_id = " + studentID + " " +
                    "AND test_id = " + testID + ";";

                SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
                DataTable studentQuizData = new DataTable();

                adapter.Fill(studentQuizData);

                return studentQuizData.Rows[0]["completed"].ToString() == "True";
            }
            catch
            {
                return false;
            }
        }

        public static TestResults GetTestResults(int studentID, int testID)
        {
            string sqlString =
                "SELECT * " +
                "FROM student_quiz " +
                    "JOIN test_question ON test_question.test_id = student_quiz.test_id "+
                "WHERE student_id = " + studentID + " " +
                    "AND student_quiz.test_id = " + testID + ";";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlString, DAL_Connection.Connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            int rightAnswerCount = 0;
            int totalAnswerCount = 0;

            for (int x = 0; x < dataTable.Rows.Count; x++)
            {
                totalAnswerCount++;
                int questionID = int.Parse(dataTable.Rows[x]["question_id"].ToString());
                if(StudentQuizQuestionDAL.AnswerIsRight(studentID,questionID))
                {
                    rightAnswerCount++;
                }
            }
            return new TestResults(rightAnswerCount, totalAnswerCount);
        }

        public static bool AllQuestionAnswered(int studentID, int testID)
        {
            DataTable testTable = TestQuestionDAL.GetDataByTestID(testID);
            List<int> questionIdList= new List<int>();
            //bool allQuestionAnswered = true;

            DataTable studentQuizTable = StudentQuizQuestionDAL.GetDataByTestID_AndStudentID(testID, studentID);
            for(int x=0;x<studentQuizTable.Rows.Count;x++)
            {
                if (studentQuizTable.Rows[x]["answer_id"].ToString() == "")
                {
                    return false;
                }
                else
                {
                    questionIdList.Add(
                        int.Parse(studentQuizTable.Rows[x]["question_id"].ToString()));
                }
            }

            for (int x = 0; x < testTable.Rows.Count; x++)
            {
                int questionID = int.Parse(testTable.Rows[x]["question_id"].ToString());
                bool questionIdExists = false;
                for (int y = 0; y < questionIdList.Count; y++)
                {
                    if (questionIdList[y] == questionID)
                    {
                        questionIdExists = true;
                    }
                    
                }
                if (!questionIdExists)
                {
                    return false;
                }
            }

            return true;
            throw new NotImplementedException();
        }
    }
}
