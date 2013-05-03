using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Configuration;

namespace GOS.Classes
{
    static class Repporting
    {

        static string exportPath =System.IO.Path.GetFullPath("..\\..\\export");

        //Code Mort

        public static bool repportCsv(DateTime debut, DateTime fin, string path)
        {
            List<Vente> listVente = Vente.getVentesByPeriod(debut, fin);

            string csv = "";

            foreach (Vente v in listVente)
            {

            }

            return true;
        }

        public static bool repportMail(DateTime debut, DateTime fin, string email)
        {

            return true;
        }

        //
        /**
         * Export la list donnée dans un fichier csv
         * TODO: Faire en sorte que ce soit générique en fonctions de la list, classes mère ?
         *         Pour ne pas faire trop de duplication de code
         */

        public static bool ExportStock(bool includeHeaderLine, List<Produit> stock)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //Get properties using reflection.
                IList<PropertyInfo> propertyInfos = typeof(Produit).GetProperties();

                if (includeHeaderLine)
                {
                    //add header line.
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        sb.Append(propertyInfo.Name).Append(";");
                    }
                    sb.Remove(sb.Length - 1, 1).AppendLine();
                }

                //add value for each property.
                foreach (Object obj in stock)
                {
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(";");
                    }
                    sb.Remove(sb.Length - 1, 1).AppendLine();
                }

                File.WriteAllText(exportPath + "\\Stocks_" + String.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".csv", sb.ToString());
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show("DEBUG: " + any.Message);
                }
                return false;
            }
            return true;
        }

        public static bool ExportClients(bool includeHeaderLine, List<Client> clients)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //Get properties using reflection.
                IList<PropertyInfo> propertyInfos = typeof(Client).GetProperties();

                if (includeHeaderLine)
                {
                    //add header line.
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        sb.Append(propertyInfo.Name).Append(";");
                    }
                    sb.Remove(sb.Length - 1, 1).AppendLine();
                }

                //add value for each property.
                foreach (Object obj in clients)
                {
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(";");
                    }
                    sb.Remove(sb.Length - 1, 1).AppendLine();
                }

                File.WriteAllText(exportPath + "\\Clients_" + String.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".csv", sb.ToString());
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show("DEBUG: " + any.Message);
                }
                return false;
            }
            return true;
        }
      
        //get the csv value for field.
        public static string MakeValueCsvFriendly(object value)
        {
            if (value == null) return "";
            if (value is Nullable && ((INullable)value).IsNull) return "";

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();

            if (output.Contains(";") || output.Contains("\""))
            {
                output = '"' + output.Replace("\"", "\"\"") + '"';
            }
            if (output.Contains("\\"))
            {
                output = '"' + output.Replace("\\", "/") + '"';
            }

            return output;

        }
    
         
    }
}
