using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GOS.Classes
{

    public class RFIDException : Exception
    {
        new public string Message;
        public RFIDException()
        {
            this.Message = "";
        }
        public RFIDException(string m)
        {
            this.Message = m;
        }
    }

    public class Client
    {

        private int ID;
        private String nom;        
        private String prenom;
        private float capital;

        public int getId()
        {
            return ID;
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

        public Client(int id, String nom, String prenom, float solde)
        {
            this.ID = id;
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

        public static Client getClientById(int id)
        {
            Client c = null;

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();

                string query = "SELECT id, nom, prenom, solde FROM client WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        if (dataReader.Read())
                        {
                            c = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3));
                        }
                    }

                    dataReader.Close();
                    return c;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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

        public bool updateClient()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                
                string query = "UPDATE client SET nom = @nom, prenom = @prenom, solde = @solde WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@solde", this.capital);
                cmd.Parameters.AddWithValue("@id", this.ID);
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


        public static Client getUserByRFID()
        {
            //Client c = new Client("testNom", "testPrenom", 50.0f);
            //int idClient = rfidGetId();
            int idClient = 1;

            try
            {
                Client cl = Client.getClientById(idClient);
                return cl;
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            return null;
        }
    }
}
