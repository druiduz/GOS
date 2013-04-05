using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GOS.Classes
{
    class Client
    {

        private int ID;
        private String nom;        
        private String prenom;
        private float capital;

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public String Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public Client(String nom, String prenom)
        {
            this.ID = this.generateID();
            this.nom = nom;
            this.prenom = prenom;
            this.capital = 0.0f;
        }

        private int generateID()
        {
            return 0;
        }

        public void addCapital(float val)
        {
            this.capital += val;
        }

        public void subCapital(float val)
        {
            this.capital -= val;
        }
    }
}
