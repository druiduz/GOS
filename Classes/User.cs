﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Configuration;

namespace GOS.Classes
{
    public class User
    {
        private int id;
        private string nom;
        private string prenom;
        private string login;

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
                            u.updateDerniereConnexion();
                        }
                    }

                    dataReader.Close();
                    return u;
                }
                catch (Exception any)
                {
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show(any.Message);
                    }
                    dataReader.Close();
                }
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
    }
}
