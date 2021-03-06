﻿using System;
using System.Text; 
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
        public string InsertPost(int userId, string post, int pointValue, string topic, bool anonymous, int? parentId)
        {
            try
            {
                List<Question> questionList = GetQuestionList();
                User currentUser = GetUser(userId);
                int currentQuestion = new int();
                if (currentUser.CurrentQuestion == questionList.Count())
                    currentQuestion = 1 - questionList.Count;
                else
                    currentQuestion = 1;
                var aQuery = new StringBuilder();
                aQuery.Append("insert into User_Posts (UserId, Post, Point_Value, Topic, Anonymous" + (parentId.HasValue ? ", ParentId) " : ") "));
                aQuery.Append($"Values ({userId}, '{post}', {pointValue}, '{topic}', {anonymous}");
                aQuery.Append(parentId.HasValue ? "," + parentId.Value + ");" : "); ");
                aQuery.Append($"Update User_Post_Points Set Post_Total = Post_Total + 1 WHERE UserId = {userId}; Update User_Post_Points Set Point_Total = Point_Total + {pointValue} WHERE UserId = {userId}; Update Users Set Current_Question = Current_Question + ({currentQuestion}) WHERE UserId = {userId};");

                //string query = "insert into User_Posts (UserId, Post, Point_Value, Topic, Anonymous, ParentId) Values (" + '"' + userId + '"' + "," + '"' + post + '"' + "," + '"' + pointValue + '"' + "," + '"' + topic + '"' + "," + anonymous + "," + (parentId.HasValue ? + " " + "'" + parentId.Value.ToString() + "'" : "NULL") + "); " +
                //    "Update User_Post_Points Set Post_Total = Post_Total + 1 WHERE UserId = " + userId + "; Update User_Post_Points Set Point_Total = Point_Total + " + pointValue + " WHERE UserId = " + userId + "; Update Users Set Current_Question = Current_Question + (" + currentQuestion + ") WHERE UserId = " + userId + ";";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(aQuery.ToString(), con);
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
        public string InsertQuestionResponse(int userId, int questionId, int responseId)
        {
            try
            {
                string query = "insert into User_Responses (UserId, QuestionId, ResponseId) Values (" + '"' + userId + '"' + "," + '"' + questionId + '"' + "," + '"' + responseId + '"' + ");";

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
            string query = "SELECT U.First_Name, U.Last_Name, P.PostId,  P.Post, P.Post_Time , P.Point_Value, P.Anonymous," +
                " P.ParentId, UP.Point_Total, P.Topic, U.IsCEO FROM Users U , User_Posts P, User_Post_Points UP WHERE U.UserId = P.UserId AND U.UserId = UP.UserId AND P.ParentId Is Null order by P.Post_Time desc;";

            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);


            string query2 = "SELECT U.First_Name, U.Last_Name, P.PostId,  P.Post, P.Post_Time , P.Point_Value, P.Anonymous," +
               " P.ParentId, UP.Point_Total, P.Topic, U.IsCEO FROM Users U , User_Posts P, User_Post_Points UP WHERE U.UserId = P.UserId AND U.UserId = UP.UserId AND P.ParentId IS NOT Null order by P.Post_Time desc;";

            MySqlConnection con2 = new MySqlConnection(getConString());

            MySqlCommand cmd2 = new MySqlCommand(query2, con2);
            MySqlDataAdapter adapter2 = new MySqlDataAdapter(cmd2);
            DataTable table2 = new DataTable();

            adapter2.Fill(table2);

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
                userPost.Anonymous = row[6].ToString();
                userPost.ParentId = null;
                userPost.PostTopic = row[9].ToString();
                userPost.UserPointTotal = Convert.ToInt32(row[8]);
                userPost.Success = true;
                userPost.IsCEO = Convert.ToBoolean(row[10]); 

                userPosts.Add(userPost);

                foreach (DataRow row2 in table2.Rows)
                {
                    UserPost userReply = new UserPost();
                    userReply.FirstName = row2[0].ToString();
                    userReply.LastName = row2[1].ToString();
                    userReply.PostId = Convert.ToInt32(row2[2]);
                    userReply.Post = row2[3].ToString();
                    userReply.PostTime = row2[4].ToString();
                    userReply.PointValue = Convert.ToInt32(row2[5]);
                    userReply.Anonymous = row2[6].ToString();
                    userReply.ParentId = Convert.ToInt32(row2[7]);
                    userReply.UserPointTotal = Convert.ToInt32(row2[8]);
                    userReply.PostTopic = row2[9].ToString();
                    userReply.Success = true;
                    userReply.IsCEO = Convert.ToBoolean(row2[10]);
                    if (userReply.ParentId == userPost.PostId)
                    {
                        userPosts.Add(userReply);
                    }
                }
            }

            return userPosts;
        }

        [WebMethod(EnableSession =true)]
        public UserPost GetPost(int postId)
        {
            var userPostModel = new UserPost();
            string query = "SELECT U.First_Name, U.Last_Name, P.PostId,  P.Post, P.Post_Time , P.Point_Value, P.Anonymous," +
                        $" P.ParentId, UP.Point_Total, P.Topic, U.IsCEO FROM Users U , User_Posts P, User_Post_Points UP WHERE U.UserId = P.UserId AND U.UserId = UP.UserId AND P.PostId={postId};";

            var con = new MySqlConnection(getConString());
            var cmd = new MySqlCommand(query, con);
            var adapater = new MySqlDataAdapter(cmd); 
            var table = new DataTable();

            adapater.Fill(table); 

            foreach(DataRow row in table.Rows)
            {
                userPostModel.FirstName = row[0].ToString();
                userPostModel.LastName = row[1].ToString();
                userPostModel.PostId = Convert.ToInt32(row[2]);
                userPostModel.Post = row[3].ToString();
                userPostModel.PostTime = row[4].ToString();
                userPostModel.PointValue = Convert.ToInt32(row[5]);
                userPostModel.Anonymous = row[6].ToString();
                userPostModel.PostTopic = row[9].ToString();
                userPostModel.UserPointTotal = Convert.ToInt32(row[8]);
                userPostModel.IsCEO = Convert.ToBoolean(row[10]); 
                userPostModel.Success = true;
            }

            return userPostModel; 
        }

        [WebMethod(EnableSession = true)]
        public User GetUser(int userId)
        {

            string query = "SELECT * FROM Users WHERE UserId='" + userId + "';";

            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            string stats = "SELECT UserId, Post_Total, Point_Total FROM User_Post_Points WHERE UserId='" + userId + "';";

            MySqlCommand cmd2 = new MySqlCommand(stats, con);
            MySqlDataAdapter adapter2 = new MySqlDataAdapter(cmd2);
            DataTable table2 = new DataTable();

            adapter2.Fill(table2);
            User user = new User();
            UserStats userStats = new UserStats();

            try
            {
                {
                    userStats.UserId = Convert.ToInt32(table2.Rows[0][0]);
                    userStats.PostTotal = Convert.ToInt32(table2.Rows[0][1]);
                    userStats.PointTotal = Convert.ToInt32(table2.Rows[0][2]);

                    user.UserID = Convert.ToInt32(table.Rows[0][0]);
                    user.UserName = table.Rows[0][1].ToString();
                    user.Alias = table.Rows[0][3].ToString();
                    user.Email = table.Rows[0][4].ToString();
                    user.FirstName = table.Rows[0][5].ToString();
                    user.LastName = table.Rows[0][6].ToString();
                    user.JobTitle = table.Rows[0][7].ToString();
                    user.CurrentQuestion = Convert.ToInt32(table.Rows[0][10]);
                    user.IsCEO = Convert.ToBoolean(table.Rows[0][11]); 
                    user.Stats = userStats;
                    user.Success = true;

                    return user;
                }
            }
            catch (Exception e)
            {
                return user;
            }
        }

        [WebMethod(EnableSession = true)]
        public Question GetQuestion()
        {
            string query = "Select * FROM Actionable_Questions;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<Question> questionList = new List<Question>();

            List<QuestionResponse> responses = GetQuestionResponses();

            foreach (DataRow row in table.Rows)
            {
                Question tempQuestion = new Question();
                tempQuestion.QuestionId = Convert.ToInt32(row[0]);
                tempQuestion.QuestionText = row[1].ToString();
                tempQuestion.Responses = responses;

                questionList.Add(tempQuestion);
            }

            Random random = new Random();
            int num = random.Next(questionList.Count());

            Question question = questionList[num];

            return question;
        }

        [WebMethod(EnableSession = true)]
        public Question GetNextQuestion(int userId)
        {
            string query = "Select * FROM Actionable_Questions;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<Question> questionList = new List<Question>();

            List<QuestionResponse> responses = GetQuestionResponses();

            foreach (DataRow row in table.Rows)
            {
                Question tempQuestion = new Question();
                tempQuestion.QuestionId = Convert.ToInt32(row[0]);
                tempQuestion.QuestionText = row[1].ToString();
                tempQuestion.Responses = responses;

                questionList.Add(tempQuestion);
            }

            User currentUser = GetUser(userId);

            Question question = questionList[currentUser.CurrentQuestion - 1];

            return question;
        }

        [WebMethod(EnableSession = true)]
        public List<Question> GetQuestionList()
        {
            string query = "Select * FROM Actionable_Questions;";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<Question> questionList = new List<Question>();

            List<QuestionResponse> responses = GetQuestionResponses();

            foreach (DataRow row in table.Rows)
            {
                Question tempQuestion = new Question();
                tempQuestion.QuestionId = Convert.ToInt32(row[0]);
                tempQuestion.QuestionText = row[1].ToString();
                tempQuestion.Responses = responses;

                questionList.Add(tempQuestion);
            }

            return questionList;
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
            string query = "Select UP.UserId, UP.Post_Total, UP.Point_Total, U.First_Name, U.Last_Name, U.IsCEO FROM User_Post_Points UP, Users U WHERE UP.UserId = U.UserId ORDER BY UP.Point_Total desc";
            MySqlConnection con = new MySqlConnection(getConString());

            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();

            adapter.Fill(table);

            List<UserStats> userStats = new List<UserStats>();
            var rank = 0;
            var previousScore = 0; 

            foreach (DataRow row in table.Rows)
            {
                UserStats user = new UserStats();
                // Ignore CEO 
                if (!Convert.ToBoolean(row[5])) {
                    user.UserId = Convert.ToInt32(row[0]);
                    user.PostTotal = Convert.ToInt32(row[1]);
                    user.PointTotal = Convert.ToInt32(row[2]);
                    user.FirstName = row[3].ToString();
                    user.LastName = row[4].ToString();
                    user.Rank = previousScore == user.PointTotal ? rank : ++rank;
                    previousScore = user.PointTotal;
                    userStats.Add(user);
                }
            }

            return userStats;
        }
    }
}
