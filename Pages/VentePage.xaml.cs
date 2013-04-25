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
        List<RecapPanier> recapPanier;
        List<Produit> liaisonPanier = new List<Produit>(); //id, produit
        private int curIndex = 0;

        public VentePage()
        {
            InitializeComponent();
            curPanier = new Vente();
            List<RecapPanier> recapPanier = new List<RecapPanier>();
        }

        public VentePage(Vente v)
        {
            InitializeComponent();
            curPanier = v;

            loadPanier();
        }

        private void loadPanier()
        {
            foreach (KeyValuePair<Produit, int> p in curPanier.getPanier())
            {
                this.grdPanier.Items.Add(new { produit = p.Key.Name, quantite = p.Value });
            }
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

            this.grdPanier.Columns.Add(new DataGridTextColumn { Header = "Produit", Width = 200, Binding = new Binding("produit") });
            this.grdPanier.Columns.Add(new DataGridTextColumn { Header = "Quantite", Binding = new Binding("quantite") });
            //this.grdPanier.Columns.Add(new DataGridTextColumn { Header = "Supprimer", Binding = new Binding("removeButt") });
            this.grdPanier.IsEnabled = false;
            this.grdPanier.CanUserSortColumns = true;
            //this.grdPanier.ItemsSource = curPanier.getPanier();
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
                MessageBox.Show("Nombre de '" + p.Name + "' insufisant");
            }
            else if (!p.checkQuantite())
            {
                //Stock.approvisionnement(p);
            }

            this.curPanier.ajoutPanier(p, 1);
            if (!liaisonPanier.Contains(p))
            {
                liaisonPanier.Add(p);
            }

            //this.grdPanier.Items.Refresh();

            #region ajout vue panier
            
            int pindex = liaisonPanier.IndexOf(p);
            if (!this.grdPanier.Items.IsEmpty && pindex < this.grdPanier.Items.Count)
            {
                object tempObject = this.grdPanier.Items.GetItemAt(pindex);
                if (tempObject != null)
                {
                    this.grdPanier.Items.RemoveAt(pindex);
                    this.grdPanier.Items.Insert(pindex, new { produit = p.Name, quantite = this.curPanier.getQuantite(p) });
                }
                else
                {
                    this.grdPanier.Items.Add(new { produit = p.Name, quantite = this.curPanier.getQuantite(p) });
                }
            }
            else
            {
                this.grdPanier.Items.Add(new { produit = p.Name, quantite = this.curPanier.getQuantite(p) });
            }
            

            #endregion

            #region tmp
            /*object t = new { produit = p.Name, quantite = this.curPanier.getQuantite(p) };
            this.recapPanier.Add(t);
            this.grdPanier.Items.Add(new { produit = p.Name, quantite = this.curPanier.getQuantite(p) });*/
            //this.grdPanier.Items.Insert(p.ID, new { produit = p.Name, quantite = this.curPanier.getQuantite(p) });


            /*if (recapPanier.ContainsKey(p))
            {
                panier[p] += q;
            }
            else
            {
                panier.Add(p, q);
            }
            
            this.grdPanier.Items.Insert(p.ID, new RecapPanier(p.Name, this.curPanier.getQuantite(p)));

            if (this.grdPanier.Items.GetItemAt(p.ID) != null)
            {
                RecapPanier tmp = (RecapPanier) this.grdPanier.Items.GetItemAt(p.ID);
                tmp.quantite += 1;
            }*/

            #endregion
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }

    public class RecapPanier
    {
        public string name;
        public int quantite;
        //public Button removeButt;

        public RecapPanier(string name, int quantite)
        {
            this.name = name;
            this.quantite = quantite;
            //this.removeButt = new Button();
        }
    }
}
