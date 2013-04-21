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
        private int vendeur_id;
        private int client_id;
        private DateTime date_vente;

        public Dictionary<Produit, int> getPanier()
        {
            return panier;
        }

        public Vente()
        {
            panier = new Dictionary<Produit,int>();
            total = 0.0f;
        }

        public Vente(float total, int vendeur_id, int client_id, DateTime date_vente)
        {
            this.total = total;
            this.vendeur_id = vendeur_id;
            this.client_id = client_id;
            this.date_vente = date_vente;
        }

        public void setPanier(Dictionary<Produit, int> panier)
        {
            this.panier = panier;
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

        public int getQuantite(Produit p)
        {
            if (panier.ContainsKey(p))
            {
                return panier[p];
            }
            else
            {
                return 0;
            }
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

        public static List<Vente> getVentesByPeriod(DateTime debut, DateTime fin)
        {
            Dictionary<Produit, int> tmpanier = new Dictionary<Produit, int>();
            List<Vente> listVente = null;

            #region bdd

            try
            {

                Connexion co = Connexion.getInstance();

                /*string query = "SELECT ve.id, ve.client_id, ve.vendeur_id, ve.date_vente, ve.total,"+
                               "vd.produit_id, vd.quantite "+
                                    "FROM vente as ve JOIN vendetails as vd ON ve.id = vd.vente_id "+
                                    "WHERE ve.date_vente BETWEEN @date_debut AND @date_fin "+
                                    "ORDER BY ve.vente_id ASC "+
                                    "GROUP BY ve.vente_id";*/
                string query = "SELECT ve.id, ve.date_vente, ve.total, " +
                               "vd.quantite, "+
                               "c.nom nom_c, c.prenom prenom_c, "+
                               "vu.nom nom_v, vu.prenom prenom_v, "+
                               "p.nom_Produit "+
                                    "FROM vente as ve "+
                                    "JOIN ventedetails as vd ON ve.id = vd.vente_id "+
                                    "JOIN client as c ON ve.client_id = c.id "+
                                    "JOIN vendeur as vu ON ve.vendeur_id = vu.id "+
                                    "JOIN produit as p ON vd.produit_id = p.idProduit "+
                                    "WHERE ve.date_vente BETWEEN '2013-04-18 22:55:00' AND '2013-04-18 23:14:50' "+
                                    "ORDER BY ve.id ASC";


                MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                cmd.Parameters.AddWithValue("@date_debut", debut.ToString());
                cmd.Parameters.AddWithValue("@date_fin", fin.ToString());
                MySqlDataReader dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    //tmpanier.Add(p
                }

                dataReader.Close();

            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }

            #endregion

            return listVente;
        }
    }
}
