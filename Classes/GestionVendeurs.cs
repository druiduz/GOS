using System;
using System.Collections;
using System.Windows;

namespace GOS.Classes
{
        public class GestionVendeurs : IEnumerable
        {
            public User[] aUsers;

            public GestionVendeurs()
            {
                this._init();
            }

            private void _init()
            {
                this.aUsers = User.getAllUsersArray();
            }

            public void store()
            {
                try
                {
                    foreach (User c in this.aUsers)
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

            public UserEnum GetEnumerator()
            {
                return new UserEnum(aUsers);
            }
        }

        public class UserEnum : IEnumerator
        {

            public User[] _users;

            int position = -1;

            public UserEnum(User[] list)
            {
                _users = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _users.Length);
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

            public User Current
            {
                get
                {
                    try
                    {
                        return _users[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    
}
