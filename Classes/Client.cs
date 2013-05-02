using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
        private String rfid_id;

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

        public string Rfid_id
        {
            get { return rfid_id; }
        }

        public Client()
        {
            this.id = -1;
            this.nom = "";
            this.prenom = "";
            this.capital = 0.0f;
            this.rfid_id = this.generateID();
            this.newClient = true;
        }

        public Client(String nom, String prenom, float solde)
        {
            this.id = -1;
            this.nom = nom;
            this.prenom = prenom;
            this.capital = solde;
            this.rfid_id = this.generateID();
            this.newClient = true;

            // TODO: ecriture de l'id sur la carte
        }

        public Client(int id, String nom, String prenom, float solde, String rfid_id)
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

        /**
         * Génére un id unique lié à une carte afin d'identifier un client 
         * 
        **/

        private string generateID()
        {
            return "";
            string rfid_id = "";
        
            do{
                //rfid_id = ""; /** TODO: RANDOM **/
                rfid_id = Utils.getRandomString(10);

                #region BDD
                try
                {
                    Connexion co = Connexion.getInstance();
                    co.checkConnexion();


                    // Test si l'identifiant unique n'est pas déjà utilisé
                    string query = "SELECT id FROM client WHERE rfid_ID = @rfidid";
                    MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                    cmd.Parameters.AddWithValue("@rfidid", rfid_id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    try
                    {
                        if (dataReader.HasRows)
                        {
                            return rfid_id;
                        }

                        dataReader.Close();
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
                    if (ConfigurationManager.AppSettings["debugmode"] == "true")
                    {
                        MessageBox.Show("DEBUG: " + "Connexion avec la base de donnée perdu");
                    }
                    return "";
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
                            c = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), "");
                        }
                    }

                    dataReader.Close();
                    return c;
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

        public static List<Client> getAllClientsList()
        {
            List<Client> lc = new List<Client>();

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, solde, rfid_ID FROM client ORDER BY id";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lc.Add(new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetString(4)));
                        }
                    }

                    dataReader.Close();
                    return lc;
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

        public static Client[] getAllClientsArray()
        {
            Client[] ac = null;
            int taille = 0;

            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();
                
                // On recupere le nombre de client pour init le tableau
                string query = "SELECT count(id) FROM client";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    taille = dataReader.GetInt32(0);
                }

                dataReader.Close();

                ac = new Client[taille];
                query = "SELECT id, nom, prenom, solde, rfid_ID FROM client ORDER BY id ASC";
                cmd = new MySqlCommand(query, co.connexion);
                dataReader = cmd.ExecuteReader();

                try
                {

                    int i = 0;
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ac[i] = new Client(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetString(4));
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
                        MessageBox.Show("DEBUG: "+any.Message);
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
            //Client cl = Client.getUserByUniqueID(idClient);
            
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
         */
        public static Client getUserByUniqueID(int id)
        {
            Client c = null;
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT id, nom, prenom, solde, rfid_id FROM client WHERE rfid_ID = @id LIMIT 1";
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
