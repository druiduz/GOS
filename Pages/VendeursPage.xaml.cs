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
    /// Logique d'interaction pour VendeursPage.xaml
    /// </summary>
    public partial class VendeursPage : Page
    {
        public List<User> vendeurs;
        public VendeursPage()
        {
            InitializeComponent();
            this.vendeurs = User.getAllUsers();
            this.grdVendeurs.ItemsSource = this.vendeurs;
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "ID", Width = 30, Binding = new Binding("ID"), IsReadOnly = true });
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "Nom", Width = 90, Binding = new Binding("Nom") });
            this.grdVendeurs.Columns.Add(new DataGridTextColumn { Header = "Prenom", Width = 90, Binding = new Binding("Prenom") });
        }
        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }
    }
}
