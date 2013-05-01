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
using System.Configuration;

namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour GestionCartePage.xaml
    /// </summary>
    public partial class GestionCartePage : Page
    {

        String nom;
        String prenom;
        float solde;

        public GestionCartePage()
        {
            InitializeComponent();

            this.nom = "";
            this.prenom = "";
            this.solde = -1.0f;
        }

        private bool validateForm()
        {

            if (nom == String.Empty)
            {
                MessageBox.Show("Veuillez renseigner le champs nom");
                return false;
            }

            if (prenom == String.Empty)
            {
                MessageBox.Show("Veuillez renseigner le champs prenom");
                return false;
            }

            if (this.solde < 0.0f)
            {
                MessageBox.Show("Veuillez renseigner le solde initial du client");
                return false;
            }

            return true;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {

            this.nom = this.txtNom.Text;
            this.prenom = this.txtPrenom.Text;
            if (this.txtSolde.Text != String.Empty)
            {
                this.solde = float.Parse(this.txtSolde.Text);
            }

            if (!this.validateForm())
            {
                return;
            }

            try
            {
                Client c = new Client(this.nom, this.prenom, this.solde);
                c.store();
                MessageBox.Show("Carte creer avec succes");

            }
            catch (Exception any)
            {

                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show(any.Message);
                }
                MessageBox.Show("Impossible de creer la carte");
                return;
            }
            
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
    }
}
