using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace GOS.Classes
{
    static class Repporting
    {
        public static bool repportCsv(DateTime debut, DateTime fin, string path)
        {
            List<Vente> listVente = Vente.getVentesByPeriod(debut, fin);


            return true;
        }

        public static bool repportMail(DateTime debut, DateTime fin, string email)
        {

            return true;
        }

    }
}
