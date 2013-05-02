using System;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using PCSC;

namespace GOS.Classes
{
    static class Utils
    {

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static bool EnvoisMails(string toList, string from, string ccList, string subject, string body)
        {

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(from);
                message.From = fromAddress;
                message.To.Add(toList);
                if (ccList != null && ccList != string.Empty)
                    message.CC.Add(ccList);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                // We use gmail as our smtp client
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(
                    "Guigui778@gmail.com", "26081988");

                smtpClient.Send(message);
                msg = "Message envoyer<BR>";
            }
            catch (Exception any)
            {
                if (ConfigurationManager.AppSettings["debugmode"] == "true")
                {
                    MessageBox.Show(any.Message);
                }
                return false;
            }
            return true;
        }

        /*public static string getRandomString(int l)
        {
            string retour = "";
            char[] listcar = char["a", "b", "c", "d", "e", "f", "g", "h", "i","j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];


            return retour;
        }*/

        public static string getRandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        #region RFID
        /**
         * Lecture des informations de la carte RFID du client
         */
        public static void readRFID()
        {
            /*** GET USER ID CARD ***/
            SCardContext ctx = new SCardContext();
            ctx.Establish(SCardScope.System);

            string[] readernames = ctx.GetReaders();
            ctx.Release();

            if (readernames == null || readernames.Length < 1)
                throw new Exception("You need at least one reader in order to run this example.");

            // Create a monitor object with its own PC/SC context.
            SCardMonitor monitor = new SCardMonitor(
                new SCardContext(),
                SCardScope.System);

            string readername = readernames[1];

            SCardReader RFIDReader = new SCardReader(ctx);
            SCardError rc = RFIDReader.Connect(
                readername,
                SCardShareMode.Shared,
                SCardProtocol.Any);

            if (rc != SCardError.Success)
            {
                Console.WriteLine("Unable to connect to RFID card / chip. Error: " +
                    SCardHelper.StringifyError(rc));
            }

            // prepare APDU
            byte[] ucByteSend = new byte[] 
            {
                0xFF,   // the instruction class
                0xCA,   // the instruction code 
                0x00,   // parameter to the instruction
                0x00,   // parameter to the instruction
                0x00    // size of I/O transfer
            };

            byte[] ucByteReceive = new byte[10];

            rc = RFIDReader.BeginTransaction();
            if (rc != SCardError.Success)
                throw new Exception("Could not begin transaction.");

            SCardPCI ioreq = new SCardPCI();    /* creates an empty object (null).
                                                 * IO returned protocol control information.
                                                 */
            IntPtr sendPci = SCardPCI.GetPci(RFIDReader.ActiveProtocol);
            rc = RFIDReader.Transmit(
                sendPci,    /* Protocol control information, T0, T1 and Raw
                             * are global defined protocol header structures.
                             */
                ucByteSend, /* the actual data to be written to the card */
                ioreq,      /* The returned protocol control information */
                ref ucByteReceive);

            if (rc == SCardError.Success)
            {
                /* on recupere l'id de la carte ici */
                Console.WriteLine("Uid carte utilisateur: " + BitConverter.ToString(ucByteReceive)); /* check in dba */
            }
            else
            {
                Console.WriteLine("Error: " + SCardHelper.StringifyError(rc));
            }

            RFIDReader.EndTransaction(SCardReaderDisposition.Leave);
            RFIDReader.Disconnect(SCardReaderDisposition.Reset);

        }

        static void CardInserted(object sender, CardStatusEventArgs args)
        {
            SCardMonitor monitor = (SCardMonitor)sender;

            Console.WriteLine(">> CardInserted Event for reader: "
                + args.ReaderName);
            Console.WriteLine("   ATR: " + StringAtr(args.Atr));
            Console.WriteLine("   State: " + args.State + "\n");
        }

        static void CardRemoved(object sender, CardStatusEventArgs args)
        {
            SCardMonitor monitor = (SCardMonitor)sender;

            Console.WriteLine(">> CardRemoved Event for reader: "
                + args.ReaderName);
            Console.WriteLine("   ATR: " + StringAtr(args.Atr));
            Console.WriteLine("   State: " + args.State + "\n");
        }

        static void Initialized(object sender, CardStatusEventArgs args)
        {
            SCardMonitor monitor = (SCardMonitor)sender;

            Console.WriteLine(">> Initialized Event for reader: "
                + args.ReaderName);
            Console.WriteLine("   ATR: " + StringAtr(args.Atr));
            Console.WriteLine("   State: " + args.State + "\n");
        }

        static void StatusChanged(object sender, StatusChangeEventArgs args)
        {
            SCardMonitor monitor = (SCardMonitor)sender;

            Console.WriteLine(">> StatusChanged Event for reader: "
                + args.ReaderName);
            Console.WriteLine("   ATR: " + StringAtr(args.ATR));
            Console.WriteLine("   Last state: " + args.LastState
                + "\n   New state: " + args.NewState + "\n");
        }

        static string StringAtr(byte[] atr)
        {
            if (atr == null)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (byte b in atr)
                sb.AppendFormat("{0:X2}", b);

            return sb.ToString();
        }
        #endregion
    }
}
