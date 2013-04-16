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

        /*private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            TabControl tabProduit = this.tabProduit;
            List<String> allTypes = Produit.getAllTypes();

            int nbColMax = 4, nbCol = 0;
            int nbRow = 0;
            int row = 0, col = 0;

            foreach (String type in allTypes)
            {
                row = 0;
                col = 0;
                List<Produit> produitByTypes = Produit.getAllProduit("where type_produit = '" + type + "'");

                if (produitByTypes.Count() > nbColMax)
                {
                    nbCol = nbColMax;
                    nbRow = (nbCol / nbColMax) + 1;
                }
                else
                {
                    nbCol = produitByTypes.Count();
                    nbRow = 1;
                }

                TabItem tabAdd = new TabItem();
                tabAdd.Header = type;
                Grid newGrid = new Grid();
                newGrid.ShowGridLines = true;


                for (int i = 0; i < nbCol; i++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    newGrid.ColumnDefinitions.Add(colDef);
                }
                for (int i = 0; i < nbRow; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    newGrid.RowDefinitions.Add(rowDef);
                }
                foreach (Produit p in produitByTypes)
                {
                    this.addProduit(newGrid, p, row, col);

                    col++;

                    if (col >= nbCol)
                    {
                        col = 0;
                        row++;
                    }
                }
               

                tabAdd.Content = newGrid;
                tabProduit.Items.Add(tabAdd);
            }

            List<Produit> allProduit = Produit.getAllProduit();
            TabItem tabAll = (TabItem) tabProduit.Items.GetItemAt(0);
            Grid allGrid = (Grid) tabAll.Content;
            allGrid.ShowGridLines = true;

            if (allProduit.Count() > nbColMax)
            {
                nbCol = nbColMax;
                nbRow = (nbCol / nbColMax) + 1;
            }
            else
            {
                nbCol = allProduit.Count();
                nbRow = 1;
            }

            col = 0;
            row = 0;

            for (int i = 0; i < nbCol; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                allGrid.ColumnDefinitions.Add(colDef);
            }
            for (int i = 0; i < nbRow; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                allGrid.RowDefinitions.Add(rowDef);
            }

            foreach (Produit p in allProduit)
            {

                this.addProduit(allGrid, p, row, col);

                col++;

                if (col >= nbCol)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private void addProduit(Grid g, Produit p, int row, int col)
        {
            TextBlock text = new TextBlock();
            text.Text = p.Name;
            Grid.SetColumn(text, col);
            Grid.SetRow(text, row);
            g.Children.Add(text);
        }*/

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
            Label l = new Label();
            l.Content = p.Name;

            Grid.SetRow(l, j);
            Grid.SetColumn(l, i);
            g.Children.Add(l);
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

        }
    }
}
