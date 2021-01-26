using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JelszoKezelo
{
    class User
    {
        private const String SERVER = "127.0.0.1";
        private const int PORT = 3306;
        private const String UID = "passwordManager";
        private const String PASSWORD = "IDontKnowWhatToSayAboutTheThingsHappendBetweenYouAndTheDatabase";
        private const String DATABASE = "passwordmanager";
        private static MySqlConnection dbConnect;

        public int Id { get; private set; }
        public String Username { get; private set; }
        public String Password { get; private set; }
        public String Masterpw { get; private set; }
        public String Database { get; private set; }

        private User(int id, String u, String p, String m)
        {
            Id = id;
            Username = u;
            Password = p;
            Masterpw = m;
        }

        public static void InitializeDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.Port = PORT;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            String connString = builder.ToString();

            dbConnect = new MySqlConnection(connString);
        }

        public static usrData GetUser(usrData user)
        {
            String query = $"SELECT * FROM users where fnev='{user.FNev}'";
            MySqlCommand cmd = new MySqlCommand(query, dbConnect);

            dbConnect.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            user.Id = -1;
            if(reader.HasRows)
            {
                reader.Read();
                user.Id = (int)reader["id"];
                user.FNev = (string)reader["fnev"];
                user.Jelszo = (string)reader["jelszo"];
            }

            dbConnect.Close();
            return user;
        }

        public static void AddUser(usrData user)
        {
            String query = $"INSERT INTO users(fnev, jelszo, masterpw) VALUES ('{user.FNev}','{user.Jelszo}','{user.MasterPw}')";
            MySqlCommand cmd = new MySqlCommand(query, dbConnect);

            dbConnect.Open();
            cmd.ExecuteNonQuery();

            dbConnect.Close();

            return;
        }

        public static void UserSessionToken(usrData user)
        {
            String query = $"UPDATE users SET sessionToken='{user.SessionToken}' WHERE fnev='{user.FNev}' AND jelszo='{user.Jelszo}'";
            MySqlCommand cmd = new MySqlCommand(query, dbConnect);

            dbConnect.Open();
            cmd.ExecuteNonQuery();
            dbConnect.Close();

            return;
        }


    }
}
