using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GOS.Classes;

namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour PaiementCartePage.xaml
    /// </summary>
    public partial class PaiementCartePage : Page
    {

        private Vente panier;
        private Client curClient;

        public PaiementCartePage(Vente panier)
        {
            InitializeComponent();
            this.panier = panier;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            curClient = Client.getUserByRFID();

            this.textNom.Text = curClient.Nom;
            this.textPrenom.Text = curClient.Prenom;
            this.textSolde.Text = curClient.getCapital().ToString();
            this.textNewSolde.Text = (curClient.getCapital() - panier.getTotal()).ToString();

            this.initRecapPanier();

            /****/
        }

        private void initRecapPanier()
        {
            this.grdRecap.Columns.Add(new DataGridTextColumn { Header = "Produit", Width = 200, Binding = new Binding("produit") });
            this.grdRecap.Columns.Add(new DataGridTextColumn { Header = "Quantite", Binding = new Binding("quantite") });

            foreach (KeyValuePair<Produit, int> p in this.panier.getPanier())
            {
                this.grdRecap.Items.Add(new { produit = p.Key.Name, quantite = p.Value });
            }



        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new VentePage(this.panier);
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {

            MainWindow main = (MainWindow)this.Parent;

            if (panier.finishVente(curClient, main.vendeur))
            {
                MessageBox.Show("Vente reussi");
            }
            else
            {
                MessageBox.Show("Echec de la vente");
            }

            HomePage home = new HomePage();
            main.Content = home;
        }
    }
}
