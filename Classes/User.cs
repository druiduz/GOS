using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GOS.Classes
{
    public class User
    {
        private int id;
        private string nom;
        private string prenom;
        private string login;

        public User(string pp)
        {

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
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();

                string query = "SELECT idClient as id, nom, prenom, login FROM vendeur WHERE login = @login AND mdp = @mdp";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@mdp", mdp);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    if (dataReader.Read())
                    {
                        return new User(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetString(3));
                    }
                }

                dataReader.Close();

            }
            catch (InvalidConnexion a)
            {

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

                string query = "UPDATE vendeur SET lastCo = NOW() WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteScalar();

            }
            catch (InvalidConnexion a)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            #endregion
        }

        public void logout()
        {

        }
    }
}
