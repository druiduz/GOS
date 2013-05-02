using System;
using System.Collections;
using System.Windows;

namespace GOS.Classes
{
    public class GestionClients : IEnumerable
    {
        public Client[] aClients;

        public GestionClients()
        {
            this._init();
        }

        private void _init()
        {
            this.aClients = Client.getAllClientsArray();
        }

        public void store()
        {
            try
            {
                foreach (Client c in this.aClients)
                {
                    if (!c.store())
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


        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ClientEnum GetEnumerator()
        {
            return new ClientEnum(aClients);
        }
    }

    public class ClientEnum : IEnumerator
    {

        public Client[] _clients;

        int position = -1;

        public ClientEnum(Client[] list)
        {
            _clients = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _clients.Length);
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

        public Client Current
        {
            get
            {
                try
                {
                    return _clients[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
