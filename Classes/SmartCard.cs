using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PCSC;
using PCSC.Iso7816;

namespace GOS.Classes
{
    class SmartCard
    {
        private SCardContext ctx;
        private SCardReader reader;
        private String uidCard;
        private String omnikeyReader;
        private IsoCard card;

        public IsoCard Card
        {
            get { return card; }
            set { card = value; }
        }

        public String OmnikeyReader
        {
            get { return omnikeyReader; }
            set { omnikeyReader = value; }
        }

        public String UidCard
        {
            get { return uidCard; }
            set { uidCard = value; }
        }

        public SCardContext Ctx
        {
            get { return ctx; }
            set { ctx = value; }
        }

        public SCardReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        public SmartCard()
        {
            // Establish PC/SC context
            this.ctx = new SCardContext();
            ctx.Establish(SCardScope.System);

            // Create a reader object
            this.reader = new SCardReader(this.ctx);

            // Use the first reader that is found
            this.omnikeyReader = ctx.GetReaders()[1];

            // Connect to the card
            this.card = new IsoCard(this.reader);

        }

        public String getUIDCard()
        {
            String uid = "";

            card.Connect(omnikeyReader, SCardShareMode.Shared, SCardProtocol.Any);

            // Build a ATR fetch case
            CommandApdu apdu = card.ConstructCommandApdu(
                IsoCase.Case2Short);

            /** test **/
            apdu.CLA = 0xFF; // Class
            apdu.INS = 0xCA; // Instruction: Recovery id
            apdu.P1 = 0x00;  // Parameter 1
            apdu.P2 = 0x00;  // Parameter 2
            apdu.Le = 0x08;  // Expected length of the returned data

            // Transmit the Command APDU to the card and receive the response
            Response resp = card.Transmit(apdu);

            byte[] data;

            // First test - get the data from all response APDUs
            data = resp.GetData();
            if (data != null)
            {
                string idCard = BitConverter.ToString(data);
                uid = idCard.Substring(0, 11).Replace("-", "");
            }

            return uid;
        }

        public void monitoring()
        {
            string readernames = ctx.GetReaders()[1];
            ctx.Release();

            // Create a monitor object with its own PC/SC context.
            SCardMonitor monitor = new SCardMonitor(
                new SCardContext(),
                SCardScope.System);

            // Point the callback function(s) to the static defined methods below.
            monitor.CardInserted += new CardInsertedEvent(CardInserted);
            monitor.CardRemoved += new CardRemovedEvent(CardRemoved);
            monitor.Initialized += new CardInitializedEvent(Initialized);
            monitor.StatusChanged += new StatusChangeEvent(StatusChanged);
            monitor.MonitorException += new MonitorExceptionEvent(MonitorException);

            Console.WriteLine("Start monitoring for reader " + reader + ".");

            monitor.Start(readernames);
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

        private void CardInserted(object sender, CardStatusEventArgs args)
        {
            SCardMonitor monitor = (SCardMonitor)sender;

            Console.WriteLine(">> CardInserted Event for reader: "
                + args.ReaderName);
            Console.WriteLine(">> UID: "
                + this.getUIDCard());
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

        static void MonitorException(object sender, PCSCException ex)
        {
            Console.WriteLine("Monitor exited due an error:");
            Console.WriteLine(SCardHelper.StringifyError(ex.SCardError));
        }
    }
}
