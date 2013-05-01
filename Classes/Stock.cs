using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GOS.Classes
{
    class Stock : IEnumerable
    {

        public List<Produit> lStock;
        public Produit[] aStock;
        //TODO Utiliser une collection perso pour que tout se gère en auto

        public Stock()
        {
            this.lStock = new List<Produit>();

            this._init();
        }

        private void _init()
        {
            lStock = Produit.getAllProduit();
            aStock = Produit.getAllProduitArray();
        }

        public void store()
        {
            try
            {

                foreach (Produit p in lStock)
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
        }

        public bool checkQuantite(int id)
        {
            return aStock[id].Quantite < aStock[id].Quantite_min;
        }
        public List<int> getOutOfOrder()
        {
            List<int> idOut = new List<int>();
            foreach(Produit p in lStock)
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
            return new StockEnum(aStock);
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
