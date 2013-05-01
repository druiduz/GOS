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
    /// Logique d'interaction pour GestionStock.xaml
    /// </summary>
    public partial class GestionStock : Page
    {

        Stock s;

        public GestionStock()
        {
            InitializeComponent();

            this.s = null;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                s = new Stock();
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show("DEBUG: " + any.Message);
                }
                MessageBox.Show("Impossible de charger la liste des produits");

                MainWindow main = (MainWindow)this.Parent;
                main.Content = new HomePage();
                return;
            }

            this.grdstock.ItemsSource = s.lStock;
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "ID", Width = 10, Binding = new Binding("ID"), IsReadOnly = true });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Name", Width = 200, Binding = new Binding("Name") });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Type", Width = 200, Binding = new Binding("Type") });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Prix", Width = 100, Binding = new Binding("Prix") });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Quantite", Width = 100, Binding = new Binding("Quantite") });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Quantite_min", Width = 100, Binding = new Binding("Quantite_min") });
            this.grdstock.Columns.Add(new DataGridTextColumn { Header = "Logo", Width = 100, Binding = new Binding("Logo") });
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s.store();
                MessageBox.Show("Enregistement réussi");
            }
            catch (Exception any)
            {
                MessageBox.Show("Erreur lors de l'enregistrement du stock");
            }
        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            Repporting.Export(true, Produit.getAllProduit());
        }

    }
}
