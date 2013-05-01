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
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public List<Client> clients;
        public HomePage()
        {
            InitializeComponent();
        }
        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {

            string nom = this.txtNom.Text;
            string prenom = this.txtPrenom.Text;
            float solde = float.Parse(this.txtSolde.Text);

            Client c = new Client(nom, prenom, solde);


            MessageBox.Show("Carte creer avec succes");

            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
        private void btnVente_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new VentePage();
        }

        private void btnCarte_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new GestionCartePage();
        }

        private void btnRapport_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new RapportPage();
        }

        private void btnStock_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new GestionStock();
        }

        private void btnDeco_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new LoginPage();
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new ClientsPage();
        }

        private void grdClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.clients = Client.getAllClients();
            this.grdClients.ItemsSource = this.clients;
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "ID", Width = 30, Binding = new Binding("ID"), IsReadOnly = true });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Nom", Width = 90, Binding = new Binding("Nom") });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Prenom", Width = 90, Binding = new Binding("Prenom") });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Solde", Width = 90, Binding = new Binding("Capital") });
   
        }
    }
}
