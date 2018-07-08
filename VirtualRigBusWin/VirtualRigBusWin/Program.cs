using System;
using HamBusLib;
using HamBusLib.UdpNetwork;
using KenwoodEmulator;
using Microsoft.Owin.Hosting;

namespace VirtualRigBusWin
{
    class Program
    {
        static private IDisposable app;
        static void Main(string[] args)
        {

            int httpPort = IpPorts.TcpPort;
            var url = string.Format("http://localhost:{0}/", httpPort);
            app = WebApp.Start(url);

            var comPort = "com20";
            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.rigBusDesc.ComPort = comPort;
            reportingThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.Id = reportingThread.Id;
            
            kenwood.OpenPort(comPort);

            var kenwood2 = new KenwoodEmu();

            Console.ReadKey();
        }
    }
}
