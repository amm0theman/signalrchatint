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
        static SQLiteConnection sqlconnect;
        public CreateDB()
            
        {
            //SQLiteConnection.CreateFile("HackerChatDB.sqlite");
            sqlconnect = new SQLiteConnection("Data Source=HackerChatDB.sqlite;Version=3");
            sqlconnect.Open();


            string sql1 = "Create TABLE UserTable (Id INTEGER PRIMARY KEY  , uname varchar(255) UNIQUE NOT NULL, pword varcher(255) NOT NULL)";
            string sql2 = "Create TABLE Logging (Id INT NOT NULL PRIMARY KEY, message varchar(255), timestamps timestamp)";
            SQLiteCommand command = new SQLiteCommand(sql1, sqlconnect);
            //command.ExecuteNonQuery();
            command = new SQLiteCommand(sql2, sqlconnect);
            //command.ExecuteNonQuery();
        }      

        static public bool LoginUser(string user, string pass)
        {
            sqlconnect = new SQLiteConnection("Data Source=HackerChatDB.sqlite;Version=3");
            sqlconnect.Open();

            //Check to see if the user and passowrd match the encrypted version
            string hasheduser = EncryptUser.HashUsernamePassword(user);
            string hashedpass = EncryptUser.HashUsernamePassword(pass);

            string sSQL = "SELECT uname, pword from Users where uname=$name";
            SQLiteCommand cmd = new SQLiteCommand(sSQL, sqlconnect);
            cmd.Parameters.AddWithValue("$name", user);
            var a = cmd.ExecuteReader();
            //var a = gay.GetString(1);
            var hashedPass = EncryptUser.ValidateUsernamePassword(pass, a[0].ToString());

            return true;
        }

        static public bool SignUpUser(string user, string pass)
        {
            EncryptUser.GetRandomSalt();
            sqlconnect = new SQLiteConnection("Data Source=HackerChatDB.sqlite;Version=3");
            sqlconnect.Open();
            int id = 0;
            id += id;
            SQLiteCommand insertCommand = new SQLiteCommand(sqlconnect);
            insertCommand.CommandText = @"INSERT INTO UserTable (uname, pword) VALUES (@uname, @pword);";
            // use parameterised queries to mitigate sql injectio
            //insertCommand.Parameters.Add(new SQLiteParameter("@id", id++));
            insertCommand.Parameters.Add(new SQLiteParameter("@uname", user));
            insertCommand.Parameters.Add(new SQLiteParameter("@pword", pass));
            insertCommand.ExecuteNonQuery();

            return true;
        }
    }
}
