using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace GOS.Classes
{
    public class Produit : IComparable
    {
        private int _ID;
        private String name;
        private String type;
        private float prix;
        private int quantite;
        private int quantite_min;
        private String logo;
        private String logoFull;

        private bool newProduit;

        public Produit()
        {
            this._ID = this.getNewProduitId();
            this.name = "undefined";
            this.prix = 0.0f;
            this.quantite = 0;
            this.quantite_min = 5;
            this.logo = "";
            this.newProduit = true;
        }

        public Produit(int id, String name, String type, float prix, int quantite, int quantite_min, String logo)
        {
            this._ID = id;
            this.name = name;
            this.type = type;
            this.prix = prix;
            this.quantite = quantite;
            this.quantite_min = quantite_min;
            
            if (!logo.Equals("")) 
            {
                this.logoFull = System.IO.Path.GetFullPath("..\\..\\Images\\logo-produit\\"+logo);
            }
            this.newProduit = false;
        }

        public Produit(int id, string name)
        {
            this._ID = id;
            this.name = name;
        }

        public int ID
        {
            get { return _ID; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        public float Prix
        {
            get { return prix; }
            set { prix = value; }
        }
        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }
        public int Quantite_min
        {
            get { return quantite_min; }
            set { quantite_min = value; }
        }
        public String Logo
        {
            get { return logo; }
            set { logo = value; }
        }

        public String LogoFull
        {
            get { return logoFull; }
            set { logoFull = value; }
        }

        public override String ToString()
        {
            String s = "";

            s += "--Object 'Produit'--'"
                + "\nId: " + this.ID
                + "\nName: " + this.name
                + "\nType: " + this.type
                + "\nPrix: " + this.prix
                + "\nQuantite: " + this.quantite
                + "\nQuantite_min: "+this.quantite_min;

            return s;
        }

        /**
         * Retourne l'id suivant de la table pour un nouveau produit
         * 
         */
        public int getNewProduitId()
        {
            int id = 0;

            #region BDD

            Connexion co = Connexion.getInstance();
            co.checkConnexion();
            string query = "SELECT idProduit FROM Produit ORDER BY idProduit DESC LIMIT 1";

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                id = dataReader.GetInt32(0)+1;
            }

            dataReader.Close();

            #endregion

            return id;
        }

        public bool store()
        {
            if (this.newProduit)
            {
                bool retour = this.create();
                if (retour)
                {
                    this.newProduit = false;
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
                string query = "INSERT INTO produit SET " +
                                "idProduit = @id, " +
                                "nom_produit = @nom, " +
                                "type_produit = @type, " +
                                "prix_produit = @prix, " +
                                "quantite = @quantite, " +
                                "quantite_mini = @quantite_min, " +
                                "logo = @logo;";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", this.ID);
                cmd.Parameters.AddWithValue("@nom", this.name);
                cmd.Parameters.AddWithValue("@type", this.type);
                cmd.Parameters.AddWithValue("@prix", this.prix);
                cmd.Parameters.AddWithValue("@quantite", this.quantite);
                cmd.Parameters.AddWithValue("@quantite_min", this.quantite_min);
                cmd.Parameters.AddWithValue("@logo", this.logo);
                cmd.ExecuteScalar();
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show(any.Message);
                }
                return false;
            }
            #endregion

            return true;
        }

        public bool update()
        {
            #region BDD
            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();
                string query = "UPDATE produit SET " +
                                "nom_produit = @nom, " +
                                "type_produit = @type, " +
                                "prix_produit = @prix, " +
                                "quantite = @quantite, " +
                                "quantite_mini = @quantite_min, " +
                                "logo = @logo " +
                                "WHERE idProduit = @idproduit";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@nom", this.name);
                cmd.Parameters.AddWithValue("@type", this.type);
                cmd.Parameters.AddWithValue("@prix", this.prix);
                cmd.Parameters.AddWithValue("@quantite", this.quantite);
                cmd.Parameters.AddWithValue("@quantite_min", this.quantite_min);
                cmd.Parameters.AddWithValue("@logo", this.logo);
                cmd.Parameters.AddWithValue("@idproduit", this.ID);
                cmd.ExecuteScalar();

            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show(any.Message);
                }
                return false;
            }
            #endregion

            return true;
        }

        public static Produit getProduit(int id)
        {
            int ID = -1;
            string name = "";
            string type = "";
            float prix = 0.0f;
            int quantite = 0;
            int quantite_min = 0;
            string logo = "";

            #region BDD
            try
            {
                Connexion co = Connexion.getInstance();
                co.checkConnexion();

                string query = "SELECT * FROM produit WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                
                try
                {
                    while (dataReader.Read())
                    {
                        ID = dataReader.GetInt32(0);
                        name = dataReader.GetString(1);
                        type = dataReader.GetString(2);
                        prix = dataReader.GetFloat(3);
                        quantite = dataReader.GetInt32(4);
                        quantite_min = dataReader.GetInt32(5);
                        logo = dataReader.GetString(6);
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
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

            Produit p = new Produit(ID, name, type, prix, quantite, quantite_min, logo);
            return p;
        }

        public static List<Produit> getAllProduit(string where = "")
        {
            List<Produit> lp = new List<Produit>();

            #region BDD

            Connexion co = Connexion.getInstance();
            co.checkConnexion();

            string query = "SELECT * FROM produit";
            if (where.Length > 0)
            {
                query += " " + where;
            }

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                lp.Add(new Produit(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetInt32(4), dataReader.GetInt32(5), dataReader.GetString(6)));
            }

            dataReader.Close();

            #endregion

            return lp;
        }

        public static Produit[] getAllProduitArray()
        {

            int taille = 0;
            Produit[] ap;

            Connexion co = Connexion.getInstance();
            co.checkConnexion();

            string query = "SELECT count(idProduit) FROM produit";
            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                taille = dataReader.GetInt32(0);
            }

            dataReader.Close();

            ap = new Produit[taille];


            query = "SELECT * FROM produit";
            cmd = new MySqlCommand(query, co.connexion);
            dataReader = cmd.ExecuteReader();

            int i = 0;
            while (dataReader.Read())
            {
                ap[i] = new Produit(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetString(2), dataReader.GetFloat(3), dataReader.GetInt32(4), dataReader.GetInt32(5), dataReader.GetString(6));
                i++;
            }

            dataReader.Close();

            return ap;
        }

        public static List<String> getAllTypes()
        {
            List<String> lp = new List<String>();

            #region BDD

            Connexion co = Connexion.getInstance();
            co.checkConnexion();

            string query = "SELECT Distinct(type_produit) FROM produit";
            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                lp.Add(dataReader.GetString(0));
            }

            dataReader.Close();

            #endregion

            return lp;
        }

        public bool checkQuantite()
        {
            return this.quantite <= this.Quantite_min;
        }

        public int CompareTo(object o)
        {
            Produit p = (Produit)o;
            return (int)(this.quantite - p.quantite);
        }
    }
}
