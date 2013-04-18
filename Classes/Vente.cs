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
            panier.Add(p, q);
        }

        public void store(int idUser)
        {
            #region BDD

            try
            {
                Connexion co = Connexion.getInstance();

                foreach (KeyValuePair<Produit, int> p in panier)
                {
                    string query = "INSERT INTO vente SET User_idUser = @userid, Produit_idProduit = @produit, Quantite_vente = @quantite, Date_Vente = NOW()";

                    MySqlCommand cmd = new MySqlCommand(query, co.connexion);
                    cmd.Parameters.AddWithValue("@userid", idUser);
                    cmd.Parameters.AddWithValue("@produit", p.Value);
                    cmd.Parameters.AddWithValue("@quantite", p.Key);
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
                c.subCapital(this.getTotal());
                c.updateClient();

                this.store(u.getId());
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
