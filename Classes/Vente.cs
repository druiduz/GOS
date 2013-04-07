using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GOS.Classes
{
    class Vente
    {
        private Dictionary<Produit, int> panier;
        
        public Vente()
        {

        }

        public void ajoutPanier(Produit p, int q)
        {
            panier.Add(p, q);
        }

        public void addVente(int idUser)
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
            catch (InvalidConnexion a)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
            }

            #endregion
        }
    }
}
