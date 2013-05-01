using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

using PCSC;

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
        private string rfid_id;
        private float solde;
        private string idCard;

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

        public float Capital
        {
            get { return capital; }
            set { capital = value; }
        }

        public Client(String nom, String prenom, float solde)
        {
            this.ID = this.generateID();
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
        }

        public Client(int id, String nom, String prenom, float solde, string rfid_id)
        {
            this.ID = id;
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
            this.rfid_id = rfid_id;
        }

        public Client(string nom, string prenom, float solde, string idCard)
        {
            this.ID = this.generateID();
            this.nom = nom;
            this.prenom = prenom;
            this.solde = solde;
            this.idCard = idCard;
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

        public static Client getClientById(string uid)
        {
            Client c = null;

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, solde FROM client WHERE idCard = @uid";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@uid", uid);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        if (dataReader.Read())
                        {
                            c = new Client(dataReader.GetInt32(0), dataReader.GetString(1),dataReader.GetString(2), dataReader.GetFloat(3), "");
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


        public static List<Client> getAllClients()
        {
            List<Client> lc = new List<Client>();

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, solde, idCard FROM client";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lc.Add(new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetString(1)));
                        }
                    }

                    dataReader.Close();
                    return lc;
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
                co.checkConnexion();

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
            Client client = null;

            SmartCard card = new SmartCard();
            String uid = card.getUIDCard();

            try
            {
                client = Client.getClientById(uid);
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            return client;
        }


        /**
         * Récupère le client grave à l'identifiant unique stocké sur la carte RFID
         * 
         */
        public static Client getUserByUniqID(int id)
        {
            Client c = null;
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();

                string query = "SELECT id, nom, prenom, solde, idCard FROM client WHERE idCard = @id LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    c = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetString(4));
                }

            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion
            return c;
        }

    }
}
