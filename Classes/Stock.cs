using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOS.Classes
{
    class Stock
    {

        private int QMin;
        private List<Produit> stock;

        public Stock()
        {
            this.stock = new List<Produit>();
            this.QMin = 1;

            this._init();
        }

        private void _init()
        {
            stock = Produit.getAllProduit();
        }

        public void afficher()
        {

        }
        public void trier()
        {

        }

        public bool checkQuantite(int id)
        {
            return stock[id].Quantite < QMin;
        }
        public List<int> getOutOfOrder()
        {
            List<int> idOut = new List<int>();
            foreach(Produit p in stock)
            {
                if(checkQuantite(p.ID)){
                    idOut.Add(p.ID);
                }
            }
            return idOut;
        }

        private void approvisionnement()
        {

        }

    }
}
