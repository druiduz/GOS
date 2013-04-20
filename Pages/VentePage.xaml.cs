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
    /// Logique d'interaction pour Vente.xaml
    /// </summary>
    public partial class VentePage : Page
    {

        private Vente curPanier;

        public VentePage()
        {
            InitializeComponent();
            curPanier = new Vente();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            loadTabAllProduit();

            List<String> allTypes = Produit.getAllTypes();

            int indexTab = 1;
            foreach (String type in allTypes)
            {
                loadTabType(type, indexTab);
                indexTab++;
            }
        }

        private void loadTabAllProduit()
        {
            int i = 0, j = 0;
            Stock s = new Stock();

            Grid myGrid = this.gridAllProduit;
            myGrid.ShowGridLines = true;

            int nbCol = 4;
            int nbRow = (s.stock.Count / nbCol) + 1;

            for (int k = 0; k < nbRow; k++)
            {
                RowDefinition rowDef1 = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef1);
            }

            for (int k = 0; k < nbCol; k++)
            {
                ColumnDefinition colDef1 = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef1);
            }

            foreach (Produit p in s.stock)
            {
                if (i >= nbCol)
                {
                    i = 0;
                    j++;
                }
                this.addProduit(myGrid, p, i, j);
                i++;
            }
        }

        private void loadTabType(String type, int indexTab)
        {
            List<Produit> listByType = Produit.getAllProduit("WHERE type_produit = '" + type + "'");

            int i = 0, j = 0;

            TabItem newTabItem = new TabItem();
            Grid myGrid = new Grid();
            myGrid.ShowGridLines = true;

            int nbCol = 4;
            int nbRow = (listByType.Count / nbCol) + 1;

            for (int k = 0; k < nbRow; k++)
            {
                RowDefinition rowDef1 = new RowDefinition();
                myGrid.RowDefinitions.Add(rowDef1);
            }

            for (int k = 0; k < nbCol; k++)
            {
                ColumnDefinition colDef1 = new ColumnDefinition();
                myGrid.ColumnDefinitions.Add(colDef1);
            }

            foreach (Produit p in listByType)
            {
                if (i >= nbCol)
                {
                    i = 0;
                    j++;
                }
                this.addProduit(myGrid, p, i, j);
                i++;
            }

            newTabItem.Content = myGrid;
            newTabItem.TabIndex = indexTab;
            newTabItem.Header = type;
            this.tabProduit.Items.Add(newTabItem);
        }

        private void addProduit(Grid g, Produit p, int i, int j)
        {
            if (p.Logo != "")
            {
                //MessageBox.Show(p.Logo);
                try
                {

                    ProduitButton child = new ProduitButton(p);
                    child.Click += new RoutedEventHandler(newBtn_Click);

                    Grid.SetRow(child, j);
                    Grid.SetColumn(child, i);
                    g.Children.Add(child);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    ProduitButton child = new ProduitButton(p);
                    child.Click += new RoutedEventHandler(newBtn_Click);

                    Grid.SetRow(child, j);
                    Grid.SetColumn(child, i);
                    g.Children.Add(child);
                }

            }
            else
            {
                Label child = new Label();
                child.Content = p.Name;

                Grid.SetRow(child, j);
                Grid.SetColumn(child, i);
                g.Children.Add(child);
            }


        }

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new HomePage();
        }

        private void btnCarte_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new PaiementCartePage(this.curPanier);
        }

        private void btnEspeces_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)this.Parent;
            main.Content = new PaiementEspecesPage(this.curPanier);
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ProduitButton button = (ProduitButton)sender;
            Produit p = button.produit;
            //MessageBox.Show(button.produit.Name);

            if (p.Quantite <= 0)
            {
                MessageBox.Show("Nombre de " + p.Name + " insufisant");
            }
            else if (!p.checkQuantite())
            {
                Stock.approvisionnement(p);
            }

            this.curPanier.ajoutPanier(p, 1);
        }
    }
}
