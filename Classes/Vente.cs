using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GOS.Classes
{
    public class Vente
    {
        private Dictionary<Produit, int> panier;
        private float total;
        
        public Vente()
        {
            panier = new Dictionary<Produit,int>();
            total = 0.0f;
        }

        public void ajoutPanier(Produit p, int q)
        {
            if (panier.ContainsKey(p))
            {
                panier[p] += q;
            }
            else
            {
                panier.Add(p, q);
            }
            this.calculTotal();
        }

        public void store(int idClient, int idUser)
        {
            #region BDD

            try
            {

                int idvente = 0;
                Connexion co = Connexion.getInstance();

                string query = "INSERT INTO vente SET client_id = @client_id, vendeur_id = @vendeur_id, Date_Vente = NOW()";

                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@client_id", idClient);
                cmd.Parameters.AddWithValue("@vendeur_id", idUser);
                cmd.ExecuteScalar();

                query = "SELECT id FROM vente ORDER BY id DESC LIMIT 1";
                cmd = new MySqlCommand(query, co.connexion);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    idvente = dataReader.GetInt32(0);
                }

                dataReader.Close();

                foreach (KeyValuePair<Produit, int> p in panier)
                {
                    query = "INSERT INTO ventedetails SET vente_id = @idvente, produit_id = @idproduit, quantite = @quantite";
                    cmd = new MySqlCommand(query, co.connexion);
                    cmd.Parameters.AddWithValue("@idvente", idvente);
                    cmd.Parameters.AddWithValue("@idProduit", p.Key.ID);
                    cmd.Parameters.AddWithValue("@quantite", p.Value);
                    cmd.ExecuteScalar();
                }
            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion
        }

        private void calculTotal()
        {
            float t = 0.0f;

            foreach (KeyValuePair<Produit, int> p in panier)
            {
                t += p.Value * p.Key.Prix;
            }

            this.total = t;
        }

        public float getTotal()
        {
            return this.total;
        }

        public bool finishVente(Client c, User u)
        {
            try
            {

                this.store(c.getId(), u.getId());
                c.subCapital(this.getTotal());
                c.updateClient();

                foreach (KeyValuePair<Produit, int> p in panier)
                {
                    p.Key.Quantite -= p.Value;
                    p.Key.update();
                }
            }
            catch (InvalidConnexion e)
            {
                return false;
            }

            return true;
        }

        public static List<Vente> getVenteByPeriod(DateTime debut, DateTime fin)
        {
            List<Vente> listVente = null;

            return listVente;
        }
    }
}
