using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GOS.Classes
{
    class Client
    {

        private int ID;
        private String nom;        
        private String prenom;
        private float capital;

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

        public float getCapital()
        {
            return this.capital;
        }

        public Client(String nom, String prenom, float solde)
        {
            this.ID = this.generateID();
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
        }

        private int generateID()
        {
            return 0;
        }

        public void addCapital(float val)
        {
            this.capital += val;
        }

        public void subCapital(float val)
        {
            this.capital -= val;
        }

        public void updateClient()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                
                string query = "UPDATE client SET nom = @nom, prenom = @prenom, solde = @solde WHERE idClient = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@solde", this.capital);
                cmd.Parameters.AddWithValue("@solde", this.ID);
                cmd.ExecuteScalar();
            }
            catch (InvalidConnexion a)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            #endregion
        }


        public static Client getUserByRFID()
        {
            Client c = new Client("testNom", "testPrenom", 50.0f);

            return c;
        }
    }
}
