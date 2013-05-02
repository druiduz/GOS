using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace GOS.Classes
{
    public class User
    {
        private int id;
        private string nom;
        private string prenom;
        private string login;
        private bool newUser;
        public int Id
        {
            get { return id; }
        }

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public String Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public String Logins
        {
            get { return login; }
            set { login = value; }
        }

        
        public User(int id, String nom, String prenom, String login)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.login = login;
        }

        public static User Login(string login, string mdp)
        {

            User u = null;
            string mdpHash = Utils.GetMd5Hash(mdp);

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, login FROM vendeur WHERE login = @login AND mdp = @mdp";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdpHash);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {
                    if (dataReader.HasRows)
                    {
                        if (dataReader.Read())
                        {
                            u = new User(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3));
                        }
                    }

                    dataReader.Close();
                    u.updateDerniereConnexion();
                    return u;
                }
                catch (Exception any)
                {
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show("DEBUG: " + any.Message);
                    }
                    dataReader.Close();
                }
            }
            catch (InvalidConnexion a)
            {
                throw a;
            }
            
            #endregion

            return null;
        }

        private void updateDerniereConnexion()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "UPDATE vendeur SET lastCo = NOW() WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.ExecuteScalar();

            }
            catch (InvalidConnexion a)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            #endregion
        }

        public int getId()
        {
            return this.id;
        }

        public static List<User> getAllUsersList()
        {
            List<User> lu = new List<User>();

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, login FROM vendeur ORDER BY id";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lu.Add(new User(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3)));
                        }
                    }

                    dataReader.Close();
                    return lu;
                }
                catch (Exception any)
                {
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show("DEBUG: " + any.Message);
                    }
                    dataReader.Close();
                }
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

            return null;
        }

        public static User[] getAllUsersArray()
        {
            User[] ac = null;
            int taille = 0;

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                // On recupere le nombre de client pour init le tableau
                string query = "SELECT count(id) FROM vendeur";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    taille = dataReader.GetInt32(0);
                }

                dataReader.Close();

                ac = new User[taille];
                query = "SELECT id, nom, prenom, login FROM vendeur ORDER BY id ASC";
                cmd = new MySqlCommand(query, co.connexion);
                dataReader = cmd.ExecuteReader();

                try
                {

                    int i = 0;
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ac[i] = new User(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3));
                            i++;
                        }
                    }

                    dataReader.Close();
                    return ac;
                }
                catch (Exception any)
                {
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show("DEBUG: " + any.Message);
                    }
                    dataReader.Close();
                }
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

            return null;
        }
        public bool create()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "INSERT INTO vendeur SET " +
                                "nom = @nom, " +
                                "prenom = @prenom, " +
                                "login = @login";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@login", this.login);
                cmd.ExecuteScalar();

                return true;
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

        }

        public bool update()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "UPDATE vendeur SET nom = @nom, prenom = @prenom, login = @login WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@login", this.login);
                cmd.ExecuteScalar();

                return true;
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

        }

        public bool store()
        {
            if (this.newUser)
            {
                bool retour = this.create();
                if (retour)
                {
                    this.newUser = false;
                }
                return retour;
            }
            else
            {
                return this.update();
            }
        }

        public override string ToString()
        {
            string s = "";

            s += "Object : Vendeur\n";
            s += "ID = '" + this.id + "'\n";
            s += "Nom = '" + this.nom + "'\n";
            s += "Prenom = '" + this.prenom + "'\n";
            s += "Login = '" + this.login + "'\n";

            return s;
        }

      
    }
}
