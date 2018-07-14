namespace VirtualKenwoodBusWin
{
    using HamBusLib;
    using HamBusLib.UdpNetwork;
    using KenwoodEmulator;
    using Microsoft.Owin.Hosting;
    using System;
    using System.Net.Http;

    /// <summary>
    /// Defines the <see cref="Program" />
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/></param>
        public static void Main(string[] args)
        {

            int httpPort = IpPorts.TcpPort;
            //var url = string.Format("http://+:{0}/", httpPort);
            var url = string.Format("http://localhost:{0}", httpPort);
            var comPort = "com20";
            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.rigBusDesc.ComPort = comPort;
            reportingThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.Id = reportingThread.Id;

            kenwood.OpenPort(comPort);

            var kenwood2 = new KenwoodEmu();
            WebApp.Start<Startup>(url: url);

            // Create HttpCient and make a request to api/values 
            Console.ReadLine();
        }

    }
}
