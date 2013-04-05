using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;

namespace GOS.Classes
{
    class Produit : IComparable
    {
        private int _ID;
        private String name;
        private int quantite;
        private float prix;

        public Produit()
        {
            this._ID = -1;
            this.name = "undefined";
            this.quantite = 0;
            this.prix = 0.0f;
        }

        public Produit(int id, String name, int quantite, float prix)
        {
            this._ID = id;
            this.name = name;
            this.quantite = quantite;
            this.prix = prix;
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
        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }
        public float Prix
        {
            get { return prix; }
            set { prix = value; }
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
            #region TODO: Connexion bdd
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
            int quantite = 0;
            float prix = 0.0f;


            #region TODO: Connexion bdd

            Connexion co = Connexion.getInstance();

            string query = "SELECT * FROM produit WHERE id = " + id;

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                ID = dataReader.GetInt32(0);
                name = dataReader.GetString(1);
                quantite = dataReader.GetInt32(2);
                prix = dataReader.GetFloat(3);
            }

            dataReader.Close();

            #endregion


            Produit p = new Produit(ID, name, quantite, prix);
            return p;
        }

        public static List<Produit> getAllProduit()
        {
            List<Produit> lp = new List<Produit>();

            #region TODO: Connexion bdd

            Connexion co = Connexion.getInstance();

            string query = "SELECT * FROM produit";

            MySqlCommand cmd = new MySqlCommand(query, co.connexion);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                lp.Add(new Produit(dataReader.GetInt32(0), dataReader.GetString(1), dataReader.GetInt32(2), dataReader.GetFloat(3)));
            }

            dataReader.Close();

            #endregion

            return lp;
        }

        public static bool addProduit(Produit p)
        {
            #region TODO: BDD

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
