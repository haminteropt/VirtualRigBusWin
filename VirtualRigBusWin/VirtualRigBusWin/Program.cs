using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamBusLib.UdpNetwork;
using KenwoodEmulator;


namespace VirtualRigBusWin
{
    class Program
    {
        static void Main(string[] args)
        {
            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.Id = reportingThread.Id;
            kenwood.OpenPort("com20");
            Console.ReadKey();
        }
    }
}
