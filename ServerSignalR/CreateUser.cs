using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ServerSignalR
{
    public class CreateUser
    {   
        public static SqlConnection OpenDBConnection()
        {
            string cn_string = Properties.Settings.Default.connection_string;

            SqlConnection cn = new SqlConnection(cn_string);
            if (cn.State != ConnectionState.Open) cn.Open();

            return cn;
        }
        
        public void Add_User(string sUName, string sPword)
        {
            sUName = sUName.Replace("'", "''");
            sPword = sPword.Replace("'", "''");

            
            string sSQL = "INSERT INTO dbo.Users ([uname],[pword]) VALUES('" + sUName + "','" + sPword + "')";

            SqlConnection cn = OpenDBConnection();

            SqlCommand cmd = new SqlCommand(sSQL, cn);
            cmd.ExecuteNonQuery();
        }

        public static void CloseDBConnection()
        {
            string cn = Properties.Settings.Default.connection_string;
            SqlConnection connection = new SqlConnection(cn);
            if (connection.State != ConnectionState.Closed) connection.Close();
        }
    }
}
