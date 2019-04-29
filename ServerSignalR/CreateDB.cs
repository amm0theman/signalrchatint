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
        public CreateDB()
        {
            SQLiteConnection.CreateFile("HackerChatDB.sqlite");
            SQLiteConnection sqlconnect;
            sqlconnect = new SQLiteConnection("Data Source=HackerChatDB.sqlite;Version=3");
            sqlconnect.Open();

            string sql1 = "Create TABLE Users (Id INT NOT NULL PRIMARY KEY, uname varchar(255) NOT NULL, pword varcher(255) NOT NULL)";
            string sql2 = "Create TABLE Logging (Id INT NOT NULL PRIMARY KEY, message varchar(MAX), timestamps timestamp, user_id int FOREIGN KEY references Users(Id)";
            SQLiteCommand command = new SQLiteCommand(sql1, sqlconnect);
            command = new SQLiteCommand(sql2, sqlconnect);
            command.ExecuteNonQuery();
        }        
    }
}
