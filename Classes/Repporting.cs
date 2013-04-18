using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace GOS.Classes
{
    static class Repporting
    {
        public static bool repport()
        {

            DateTime debut = new DateTime(), fin = new DateTime();
            List<Vente> listVente = Vente.getVenteByPeriod(debut, fin);

            return true;
        }

    }
}
