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
            var comPort = "com20";
            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.rigBusDesc.ComPort = comPort;
            reportingThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.Id = reportingThread.Id;
            
            kenwood.OpenPort(comPort);
            Console.ReadKey();
        }
    }
}
