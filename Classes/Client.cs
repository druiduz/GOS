using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

using PCSC;
using System.Configuration;

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

        private int id;
        private String nom;        
        private String prenom;
        private float capital;
        private int rfid_id;

        private bool newClient;

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

        public float Capital
        {
            get { return capital; }
            set { capital = value; }
        }

        public int Rfid_id
        {
            get { return rfid_id; }
        }

        public Client(String nom, String prenom, float solde)
        {
            this.id = this.generateID();
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
            this.newClient = true;
        }

        public Client(int id, String nom, String prenom, float solde, int rfid_id)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
            this.rfid_id = rfid_id;
            this.newClient = false;
        }

        public override string ToString()
        {
            string s = "";

            s += "Object : Client\n";
            s += "ID = '" + this.id + "'\n";
            s += "Nom = '" + this.nom + "'\n";
            s += "Prenom = '" + this.prenom + "'\n";
            s += "Solde = '" + this.capital + "'\n";
            s += "Rfid_id = '" + this.rfid_id + "'\n";

            return s;
        }

        private int generateID()
        {
            return 0;
            int rfid_id = -1;
        
            do{
                rfid_id = 0; /** RANDOM **/

                #region BDD
                try
                {
                    Connexion co = Connexion.getInstance();
                    co.checkConnexion();

                    string query = "SELECT id FROM client WHERE rfid_ID = @rfidid";
                    MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                    cmd.Parameters.AddWithValue("@rfidid", rfid_id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    try
                    {
                        if (dataReader.Read())
                        {
                            return rfid_id;
                        }

                        dataReader.Close();
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
                catch (InvalidConnexion e)
                {
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show("Connexion avec la base de donnée perdu");
                    }
                    return 0;
                }
            
                #endregion

            } while( true );

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
                co.checkConnexion();

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
                            c = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), 0);
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

                string query = "SELECT id, nom, prenom, solde, rfid_ID FROM client";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lc.Add(new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetInt32(4)));
                        }
                    }

                    dataReader.Close(); 
                    return lc;
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
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

            return null;
        }

        public bool store()
        {
            if (this.newClient)
            {
                bool retour = this.create();
                if (retour)
                {
                    this.newClient = false;
                }
                return retour;
            }
            else
            {
                return this.update();
            }
        }

        public bool create()
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "INSERT INTO client SET " +
                                "nom = @nom, " +
                                "prenom = @prenom, " +
                                "solde = @solde, " +
                                "rfid_id = @rfid_id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@solde", this.capital);
                cmd.Parameters.AddWithValue("@rfid_id", this.rfid_id);
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

                string query = "UPDATE client SET nom = @nom, prenom = @prenom, solde = @solde WHERE id = @id";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.nom);
                cmd.Parameters.AddWithValue("@prenom", this.prenom);
                cmd.Parameters.AddWithValue("@solde", this.capital);
                cmd.Parameters.AddWithValue("@id", this.id);
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
            //Client cl = Client.getUserByUniqID(idClient);
            
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

                string query = "SELECT id, nom, prenom, solde, rfid_id FROM client WHERE rfid_ID = @id LIMIT 1";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    c = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetInt32(4));
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
