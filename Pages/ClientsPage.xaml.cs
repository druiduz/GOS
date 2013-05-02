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
using System.Collections;
using GOS.Classes;
namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public GestionClients gClients;

        public ClientsPage()
        {
            InitializeComponent();

            this.gClients = null;
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.gClients = new GestionClients();
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show("DEBUG: " + any.Message);
                }
                MessageBox.Show("Impossible de charger la liste des clients");

                MainWindow main = (MainWindow)this.Parent;
                main.Content = new HomePage();
                return;
            }

            this.grdClients.ItemsSource = this.gClients.aClients;
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "ID", Width = 40, Binding = new Binding("Id"), IsReadOnly = true });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Nom", Width = 150, Binding = new Binding("Nom") });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Prenom", Width = 150, Binding = new Binding("Prenom") });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "Solde", Width = 90, Binding = new Binding("Capital") });
            this.grdClients.Columns.Add(new DataGridTextColumn { Header = "RFID", Width = 300, Binding = new Binding("Rfid_id"), IsReadOnly = true });
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gClients.store();
                MessageBox.Show("Enregistement réussi");
            }
            catch (Exception any)
            {
                MessageBox.Show("Erreur lors de l'enregistrement des clients");
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (Repporting.ExportClients(true, Client.getAllClientsList()))
            {
                MessageBox.Show("Export des clients réussi !");
            }
            else
            {
                MessageBox.Show("Un problème est survenue durant l'export des clients");
            }
        }
    }
}

