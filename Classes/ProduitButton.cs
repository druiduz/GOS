using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GOS.Classes
{
    class ProduitButton : Button
    {
        public Produit produit;

        public ProduitButton(Produit p) : base()
        {
            this.produit = p;

            this.initContent();
        }

        private void initContent()
        {
            try
            {
                BitmapImage biImage = new BitmapImage();
                biImage.BeginInit();
                biImage.UriSource = new Uri(produit.LogoFull);
                biImage.EndInit();

                StackPanel spPanel = new StackPanel() { Orientation = System.Windows.Controls.Orientation.Vertical };
                spPanel.Children.Add(new Image() { Source = biImage });

                this.Content = spPanel;
            }
            catch (Exception e)
            {
                this.Content = produit.Name;
            }
        }
    }
}
