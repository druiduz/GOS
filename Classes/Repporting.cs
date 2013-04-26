using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Data.SqlTypes;
using System.Reflection;
using System.IO;

namespace GOS.Classes
{
    static class Repporting
    {
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

        
        public  static void Export(bool includeHeaderLine,List<Produit> stock)
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
            foreach (Produit obj in stock)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(";");
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            
            File.WriteAllText("D:/Stocks_"+DateTime.Now.ToString()+".csv", sb.ToString());
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
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;

        }
    
         
    }
}
