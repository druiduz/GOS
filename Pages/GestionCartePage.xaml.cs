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
    /// Logique d'interaction pour GestionCartePage.xaml
    /// </summary>
    public partial class GestionCartePage : Page
    {
        public GestionCartePage()
        {
            InitializeComponent();
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

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
    }
}
