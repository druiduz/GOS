using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;

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

        public Produit()
        {
            this._ID = -1;
            this.name = "undefined";
            this.quantite = 0;
            this.prix = 0.0f;
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
                this.logo = "E:/Codage/GOS/Images/" + logo;
            }
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

        public override String ToString()
        {
            String s = "";

            s += "--Object 'Produit'--'\n";
            s += "name: "+name+"\nquantite: "+quantite+"\nprix: "+prix;

            return s;
        }


        public void update()
        {
            #region BDD
            Connexion co = Connexion.getInstance();

            string query = "UPDATE produit SET name = @name, quantite = @quantite, prix = @prix WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            cmd.Parameters.AddWithValue("@name", this.name);
            cmd.Parameters.AddWithValue("@quantite", this.quantite);
            cmd.Parameters.AddWithValue("@prix", this.prix);
            cmd.Parameters.AddWithValue("@id", this.ID);
            cmd.ExecuteScalar();

            #endregion
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

            Connexion co = Connexion.getInstance();

            string query = "SELECT * FROM produit WHERE id = " + id;

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

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

            #endregion


            Produit p = new Produit(ID, name, type, prix, quantite, quantite_min, logo);
            return p;
        }

        public static List<Produit> getAllProduit(string where = "")
        {
            List<Produit> lp = new List<Produit>();

            #region BDD

            Connexion co = Connexion.getInstance();

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

        public static List<String> getAllTypes()
        {
            List<String> lp = new List<String>();

            #region BDD

            Connexion co = Connexion.getInstance();

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

        public static bool addProduit(Produit p)
        {
            #region BDD

            Connexion co = Connexion.getInstance();

            string query = "INSERT INTO produit SET name = @name, quantite = @quantite, prix = @prix";

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            cmd.Parameters.AddWithValue("@name", p.name);
            cmd.Parameters.AddWithValue("@quantite", p.quantite);
            cmd.Parameters.AddWithValue("@prix", p.prix);
            cmd.ExecuteScalar();

            #endregion

            return true;
        }

        public int CompareTo(object o)
        {
            Produit p = (Produit)o;
            return (int)(this.quantite - p.quantite);
        }
    }
}
