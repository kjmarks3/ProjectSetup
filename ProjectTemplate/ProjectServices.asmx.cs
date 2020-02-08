using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

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
        public string InsertUser(User user)
        {
            try
            {
                string query = "insert into User (CustomerID, Username, Alias) Values (" + user.CustomerID + "," + user.UserName + "," + user.Alias + ")";

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
        public string InsertPost(string post)
        {
            try
            {
                string query = "insert into Posts (Post) Values (" + post + ")";

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
        public string ValidateUser(User user)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM login WHERE username='" + user.UserName + "' AND password='" + user.Password + "'";

                MySqlConnection con = new MySqlConnection(getConString());

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();

                if (table.Rows[0][0].ToString() == "1")
                {
                    //put home page here to redirect
                    return "Success!";
                }
                else
                {
                    return "User does not exist";
                }
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }
    }
}
