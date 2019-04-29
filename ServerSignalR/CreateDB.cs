using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSignalR
{
    class CreateDB
    {
        SQLiteConnection sqlconnect;
        public CreateDB()
        {
            SQLiteConnection.CreateFile("HackerChatDB.sqlite");
            
            sqlconnect = new SQLiteConnection("Data Source=HackerChatDB.sqlite;Version=3");
            sqlconnect.Open();

            string sql1 = "Create TABLE Users (Id INT NOT NULL PRIMARY KEY, uname varchar(255) NOT NULL, pword varcher(255) NOT NULL)";
            string sql2 = "Create TABLE Logging (Id INT NOT NULL PRIMARY KEY, message varchar(255), timestamps timestamp)";
            SQLiteCommand command = new SQLiteCommand(sql1, sqlconnect);
            command = new SQLiteCommand(sql2, sqlconnect);
            command.ExecuteNonQuery();
        }      
        
        public void CreateUser(string sUName, string sPword)
        {
            sUName = sUName.Replace("'", "''");
            sPword = sPword.Replace("'", "''");

            string hashedUname = EncryptUser.HashUsernamePassword(sUName);
            string hashedPword = EncryptUser.HashUsernamePassword(sPword);

            string sSQL = "INSERT INTO dbo.Users ([uname],[pword]) VALUES('" + hashedUname + "','" + hashedPword + "')";

            SQLiteCommand cmd = new SQLiteCommand(sSQL);
            cmd.ExecuteNonQuery();
        }

        public void LogInUser(string input_uname, string input_pword)
        {

        }
    }
}
