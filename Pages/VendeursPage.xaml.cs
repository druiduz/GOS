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

namespace GOS.Pages
{
    /// <summary>
    /// Logique d'interaction pour VendeursPage.xaml
    /// </summary>
    public partial class VendeursPage : Page
    {
        public GestionVendeurs gVendeurs;

        public VendeursPage()
        {
            InitializeComponent();

            this.gVendeurs = null;
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
                this.gVendeurs = new GestionVendeurs();
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show("DEBUG: " + any.Message);
                }
                MessageBox.Show("Impossible de charger la liste des vendeurs");

                MainWindow main = (MainWindow)this.Parent;
                main.Content = new HomePage();
                return;
            }

            this.grdVendeurs.ItemsSource = this.gVendeurs.aUsers;
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "ID", Width = 40, Binding = new Binding("Id"), IsReadOnly = true });
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "Nom", Width = 150, Binding = new Binding("Nom") });
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "Prenom", Width = 150, Binding = new Binding("Prenom") });
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "Login", Width = 90, Binding = new Binding("Login") });
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gVendeurs.store();
                MessageBox.Show("Enregistement réussi");
            }
            catch (Exception any)
            {
                MessageBox.Show("Erreur lors de l'enregistrement des clients");
            }
        }

        private void grdVendeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    
}
