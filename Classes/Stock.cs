using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GOS.Classes
{
    class Stock : IEnumerable
    {

        public List<Produit> stock;
        public Produit[] _stock;
        //TODO Utiliser une collection perso pour que tout se gère en auto

        public Stock()
        {
            this.stock = new List<Produit>();

            this._init();
        }

        private void _init()
        {
            stock = Produit.getAllProduit();
            _stock = Produit.getAllProduitArray();
        }

        public void afficher()
        {

        }
        public void trier()
        {

        }

        public void store()
        {
            #region BDD

            try
            {

                foreach (Produit p in stock)
                {
                    if (!p.store())
                    {
                        throw new Exception();
                    }
                }

            }
            catch (InvalidConnexion e)
            {
                MessageBox.Show("Connexion avec la base de donnée perdu");
                throw e;
            }
            catch (Exception any)
            {
                throw any;
            }

            #endregion
        }

        public bool checkQuantite(int id)
        {
            return stock[id].Quantite < stock[id].Quantite_min;
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

        public static void checkApprovisionnement()
        {
            List<Produit> toApp = new List<Produit>();
            List<Produit> listProduit = Produit.getAllProduit();
            foreach (Produit p in listProduit)
            {
                if (p.Quantite <= p.Quantite_min)
                {
                    toApp.Add(p);
                }
            }

            Stock.approvisionnement(toApp);
        }

        public static void approvisionnement(List<Produit> lp)
        {

        }

        private void doAllApprovisionnement()
        {

        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public StockEnum GetEnumerator()
        {
            return new StockEnum(_stock);
        }
    }

    public class StockEnum : IEnumerator
    {

        public Produit[] _produit;

        int position = -1;

        public StockEnum(Produit[] list)
        {
            _produit = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _produit.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Produit Current
        {
            get
            {
                try
                {
                    return _produit[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
