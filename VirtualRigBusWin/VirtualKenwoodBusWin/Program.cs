namespace VirtualKenwoodBusWin
{
    using HamBusLib;
    using HamBusLib.Models.Configuration;
    using HamBusLib.UdpNetwork;
    using KenwoodEmulator;
    using Microsoft.Owin.Hosting;
    using System;
    using System.IO.Ports;
    using System.Net.Http;
    using System.Threading;

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


            var url = string.Format("http://+:{0}", httpPort);

            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.StartInfoThread();
            StartVirtualRigs();

            WebApp.Start<Startup>(url: url);
            Console.ReadLine();
        }

        private static void StartVirtualRigs()
        {
            var virtRigIngfo = VirtualRigInfo.Instance.GetVirtualRigConfig();
            foreach (var port in virtRigIngfo.CommPorts)
            {
                var kenwoodVRThread = new Thread(()=>StartKenwoodVR(port));
                kenwoodVRThread.Start();
            }
        }

        private static void StartKenwoodVR(CommPortConf commPortConf)
        {
            var kenwood = new KenwoodEmu();
            kenwood.ThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Starting Virtual Rig on Port: {1}: {0}",
                commPortConf.PortName, commPortConf.DisplayName);
            kenwood.OpenPort(commPortConf);
        }
    }
}
