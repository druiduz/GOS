using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GOS.Classes
{

    public class InvalidConnexion : Exception
    {
        new public string Message;
        public InvalidConnexion()
        {
            this.Message = "";
        }
        public InvalidConnexion(string m)
        {
            this.Message = m;
        }
    }

    // Non utilisé
    public class QueryException : Exception
    {
        public string query;
        new public string Message;
        public QueryException(string query, string m)
        {
            this.query = query;
            this.Message = m;
        }
    }

    class Connexion
    {
        static private Connexion instance;
        public MySqlConnection connexion;
        private string server;
        private string database;
        private string uid;
        private string password;

        private Connexion()
        {
            Initialize();
            if (!OpenConnection()) { throw new InvalidConnexion("Impossible d'ouvrir la connexion"); }
        }

        public static Connexion getInstance()
        {
            if (Connexion.instance != null) return Connexion.instance;
            else
            {
                instance = new Connexion();
                return instance;
            }
        }

        ~Connexion()
        {
            this.CloseConnection();
        }


        /*
         * Initialize values
        **/
        private void Initialize()
        {
            server = ConfigurationManager.AppSettings["bdd_server"];
            database = ConfigurationManager.AppSettings["bdd_database"];
            uid = ConfigurationManager.AppSettings["bdd_uid"];
            password = ConfigurationManager.AppSettings["bdd_password"];
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connexion = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connexion.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
            return false;
        }

        private bool CloseConnection()
        {
            try
            {
                connexion.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /**
         * Vérifie si la connexion est encore ouverte
         */
        public void checkConnexion()
        {
            if (!connexion.Ping())
            {
                throw new InvalidConnexion();
            }
        }
    }
}
