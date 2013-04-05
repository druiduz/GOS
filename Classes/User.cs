using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace GOS.Classes
{
    public class User
    {
        private string nom;
        private string prenom;
        private string login;

        public User(string pp)
        {

        }

        public User(String nom, String prenom, String login)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.login = login;
        }

        public static User Login(string login, string mdp)
        {
            #region BDD

            /*try
            {*/
                Connexion co = Connexion.getInstance();

                string query = "SELECT nom, prenom, login FROM vendeur WHERE login = @login AND mdp = @mdp";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdp);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        return new User(dataReader.GetString(0), dataReader.GetString(1), dataReader.GetString(2));
                    }
                }

                dataReader.Close();

            /*}
            catch (InvalidConnexion a)
            {

            }*/
            #endregion

            return null;
        }

        public void logout()
        {

        }
    }
}
