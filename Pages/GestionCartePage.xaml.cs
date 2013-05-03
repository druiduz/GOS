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
        String rfid;

        public GestionCartePage()
        {
            InitializeComponent();

            this.nom = "";
            this.prenom = "";
            this.solde = -1.0f;
            this.rfid = "";
        }

        private bool validateForm()
        {

            if (String.IsNullOrEmpty(rfid))
            {
                MessageBox.Show("Veuillez analyser une carte");
                return false;
            }

            if (Client.checkRfidUsed(rfid))
            {
                MessageBox.Show("L'identifiant RFID est déjà assigné à un client");
                return false;
            }

            if (String.IsNullOrEmpty(nom))
            {
                MessageBox.Show("Veuillez renseigner le champs nom");
                return false;
            }

            if (String.IsNullOrEmpty(prenom))
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
                this.solde = float.Parse(this.txtSolde.Text.Replace('.', ','));
            }
            this.rfid = this.txtRfid.Text;

            if (!this.validateForm())
            {
                return;
            }

            try
            {
                Client c = new Client(this.nom, this.prenom, this.solde, this.rfid);
                c.store();
                MessageBox.Show("Client créé avec succes");
                MainWindow main = (MainWindow)this.Parent;
                main.Content = new HomePage();
            }
            catch (Exception any)
            {

                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show(any.Message);
                }
                MessageBox.Show("Impossible de créer client");
                return;
            }
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtNom.Focus();
        }

        private void btnAnalyse_Click(object sender, RoutedEventArgs e)
        {
            String rfid_id = "";
            try
            {
                SmartCard card = new SmartCard();            
                rfid_id = card.getUIDCard();
                this.txtRfid.Text = rfid_id;
                this.enableAll();
            }
            catch (RFIDException rfidexcept)
            {
                MessageBox.Show(rfidexcept.Message);
            }
        }

        private void enableAll()
        {
            this.txtNom.IsEnabled = true;
            this.txtPrenom.IsEnabled = true;
            this.txtSolde.IsEnabled = true;

        }
    }
}
