using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//using MySQL.Data;
using MySql.Data.MySqlClient;
using System.Data;
using ProjectTemplate.Models;

namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "scrumdevils";
        private string dbPass = "!!Scrumdevils440";
        private string dbName = "scrumdevils";
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        ///call this method anywhere that you need the connection string!
        ////////////////////////////////////////////////////////////////////////
        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
        }

        [WebMethod(EnableSession = true)]

        public string TestConnection()
        {
            try
            {
                string testQuery = "select * from test";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again. Error: " + e.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string InsertUser(string username, string alias, string email)
        {
            try
            {
                string query = "insert into Users (User_Name, Alias, Email) Values (" + '"' + username + '"' + "," + '"' + alias + '"' + "," + '"' + email + '"' + ")";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }
        [WebMethod(EnableSession = true)]
        public string InsertPost(int userId, string post, int pointValue, string topic, bool anonymous)
        {
            try
            {
                string query = "insert into User_Posts (UserId, Post, Point_Value, Topic, Anonymous) Values (" + '"' + userId + '"' + "," + '"' + post + '"' + "," + '"' + pointValue + '"' + "," + '"' + topic + '"' + "," + '"' + anonymous + '"' + "); " +
                    "Update User_Post_Points Set Post_Total = Post_Total + 1 WHERE UserId = " + userId + "; Update User_Post_Points Set Point_Total = Point_Total + " + pointValue + " WHERE UserId = " + userId + ";";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public User ValidateUser(string username, string password)
        {

            string query = "SELECT * FROM Users WHERE User_Name='" + username + "' AND Password='" + password + "'";

            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);
            User user = new User();

            try
            {
                if (table.Rows[0][1].ToString() == username && table.Rows[0][2].ToString() == password)
                {
                    //put home page here to redirec

                    user.UserID = Convert.ToInt32(table.Rows[0][0]);
                    user.UserName = table.Rows[0][1].ToString();
                    user.Alias = table.Rows[0][3].ToString();
                    user.Email = table.Rows[0][4].ToString();
                    user.FirstName = table.Rows[0][5].ToString();
                    user.LastName = table.Rows[0][6].ToString();
                    user.JobTitle = table.Rows[0][7].ToString();
                    user.Success = true;

                    return user;
                }
                else
                {
                    user.Success = false;
                    user.ErrorMessage = "Something went wrong, please check your credentials and db name and try again.";
                    return user;
                }
            }
            catch (Exception e)
            {
                return user;
            }
        }
        [WebMethod(EnableSession = true)]
        public List<UserPost> ViewPosts()
        {
            string query = "SELECT U.First_Name, U.Last_Name, P.PostId,  P.Post, P.Post_Time , P.Point_Value FROM Users U , User_Posts P WHERE U.UserId = P.UserId order by P.Post_Time";

            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<UserPost> userPosts = new List<UserPost>();

            foreach (DataRow row in table.Rows)
            {
                UserPost userPost = new UserPost();
                userPost.FirstName = row[0].ToString();
                userPost.LastName = row[1].ToString();
                userPost.PostId = Convert.ToInt32(row[2]);
                userPost.Post = row[3].ToString();
                userPost.PostTime = row[4].ToString();
                userPost.PointValue = Convert.ToInt32(row[5]);

                userPosts.Add(userPost);
            }


            return userPosts;
        }

        [WebMethod(EnableSession = true)]
        public Question GetQuestion()
        {
            string query = "Select Question_Text FROM Actionable_Questions;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<string> questionList = new List<string>();

            foreach (DataRow row in table.Rows)
            {
                questionList.Add(row[0].ToString());
            }

            Random random = new Random();
            int num = random.Next(questionList.Count());

            Question question = new Question()
            {
                QuestionText = questionList[num],
                Responses = GetQuestionResponses()
            };

            return question;
        }

        [WebMethod(EnableSession = true)]
        public List<QuestionResponse> GetQuestionResponses()
        {
            string query = "Select * FROM Question_Responses;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<QuestionResponse> questionResponses = new List<QuestionResponse>();

            foreach (DataRow row in table.Rows)
            {
                QuestionResponse response = new QuestionResponse();
                response.ResponseId = Convert.ToInt32(row[0]);
                response.ResponseValue = row[1].ToString();

                questionResponses.Add(response);
            }

            return questionResponses;
        }

        [WebMethod(EnableSession = true)]
        public List<PostTopic> GetPostTopics()
        {
            string query = "Select * FROM Post_Topics;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<PostTopic> postTopics = new List<PostTopic>();

            foreach (DataRow row in table.Rows)
            {
                PostTopic topic = new PostTopic();
                topic.TopicId = Convert.ToInt32(row[0]);
                topic.Topic = row[1].ToString();

                postTopics.Add(topic);
            }

            return postTopics;
        }

        [WebMethod(EnableSession = true)]
        public List<UserStats> GetUserStats()
        {
            string query = "Select UserId, Post_Total, Point_Total FROM User_Post_Points;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<UserStats> userStats = new List<UserStats>();

            foreach (DataRow row in table.Rows)
            {
                UserStats user = new UserStats();
                user.UserId = Convert.ToInt32(row[0]);
                user.PostTotal = Convert.ToInt32(row[1]);
                user.PointTotal = Convert.ToInt32(row[2]);

                userStats.Add(user);
            }

            return userStats;
        }
    }
}
