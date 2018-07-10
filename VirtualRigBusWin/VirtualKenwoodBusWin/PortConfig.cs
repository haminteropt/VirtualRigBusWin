
using System.IO.Ports;


namespace VirtualKenwoodBusWin
{
    public class PortConfig
    {
        public string DisplayName { get; set; }
        public string PortName {get;set;}
        public int BaudRate { get; set; }
        public string Parity { get; set; }
        public int DataBits { get; set; }
        public decimal StopBits { get; set; }
        public string Handshake { get; set; }
        public int ReadTimeout { get; set; }
        public int WriteTimeout { get; set; }

        public Parity GetParity(string p)
        {
            Parity rc = System.IO.Ports.Parity.None;
            var parity = p.ToLower();
            switch (parity)
            {
                case "even":
                    rc = System.IO.Ports.Parity.Even;
                    break;
                case "odd":
                    rc = System.IO.Ports.Parity.Odd;
                    break;
                case "none":
                    rc = System.IO.Ports.Parity.None;
                    break;
            }
            return rc;
        }
        public StopBits GetStop(decimal s)
        {
            StopBits rc = System.IO.Ports.StopBits.One;

            switch (s)
            {
                case 1:
                    rc = System.IO.Ports.StopBits.One;
                    break;
                case 2:
                    rc = System.IO.Ports.StopBits.Two;
                    break;
                case 0:
                    rc = System.IO.Ports.StopBits.None;
                    break;
                case 1.5m:
                    rc = System.IO.Ports.StopBits.OnePointFive;
                    break;
            }
            return rc;
        }
    }
}
