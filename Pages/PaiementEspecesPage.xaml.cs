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
    /// Logique d'interaction pour PaiementEspecesPage.xaml
    /// </summary>
    public partial class PaiementEspecesPage : Page
    {
        Vente panier;
        string monnaieTxt;
        float monnaieFloat;
        float rendu;

        public PaiementEspecesPage(Vente panier)
        {
            InitializeComponent();
            this.panier = panier;
            this.monnaieTxt = "";
            this.rendu = 0.0f;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtPrix.Text = panier.getTotal().ToString();
            this.txtMonnaie.Text = this.monnaieTxt;
            //this.textSolde.Text = curClient.getCapital().ToString();

            this.initRecapPanier();
        }

        private void initRecapPanier()
        {
            this.grdPanier.Columns.Add(new DataGridTextColumn { Header = "Produit", Width = 200, Binding = new Binding("produit") });
            this.grdPanier.Columns.Add(new DataGridTextColumn { Header = "Quantite", Binding = new Binding("quantite") });

            foreach (KeyValuePair<Produit, int> p in this.panier.getPanier())
            {
                this.grdPanier.Items.Add(new { produit = p.Key.Name, quantite = p.Value });
            }
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new VentePage(this.panier);
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {

            if (this.monnaieTxt == "" || float.Parse(this.monnaieTxt) < float.Parse(this.txtPrix.Text))
            {
                MessageBox.Show("Montant insufisant");
                return;
            }

            MainWindow main = (MainWindow)this.Parent;

            if (panier.finishVente(null, main.vendeur, float.Parse(this.txtRendu.Text)))
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

        private void updateChamps()
        {
            this.txtMonnaie.Text = this.monnaieTxt;
            this.rendu = float.Parse(this.txtMonnaie.Text) - float.Parse(this.txtPrix.Text);
            this.txtRendu.Text = this.rendu.ToString();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "1";
            this.updateChamps();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "2";
            this.updateChamps();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "3";
            this.updateChamps();
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "4";
            this.updateChamps();
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "5";
            this.updateChamps();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "6";
            this.updateChamps();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "7";
            this.updateChamps();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "8";
            this.updateChamps();
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "9";
            this.updateChamps();
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += "0";
            this.updateChamps();
        }

        private void btnvirg_Click(object sender, RoutedEventArgs e)
        {
            this.monnaieTxt += ",";
            //this.updateChamps();
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            if (this.monnaieTxt == "") return;

            this.monnaieTxt = this.monnaieTxt.Remove(this.monnaieTxt.Length - 1);
            if (this.monnaieTxt != "")
            {
                this.updateChamps();
            }
        }

    }
}
