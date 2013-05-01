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
        public HomePage()
        {
            InitializeComponent();  
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


        private void Grid_Initialized_GestionCarte(object sender, EventArgs e)
        {
            frameGestionCarte.Content = new GestionCartePage();
        }


        private void Grid_Initialized_Stock(object sender, EventArgs e)
        {
            frameStock.Content = new GestionStock();
        }
        

        private void Grid_Initialized_Vente(object sender, EventArgs e)
        {
            frameVente.Content = new VentePage();
        }

        private void Grid_Initialized_Clients(object sender, EventArgs e)
        {
            frameClients.Content = new ClientsPage();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void frameGestionCarte_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void frameVenteCarte_Navigated(object sender, NavigationEventArgs e)
        {

        }



    
   

     
    }
}
