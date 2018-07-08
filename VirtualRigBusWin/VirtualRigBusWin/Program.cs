using System;
using System.Net.Http;
using HamBusLib;
using HamBusLib.UdpNetwork;
using KenwoodEmulator;
using Microsoft.Owin.Hosting;

namespace VirtualKenwoodBusWin
{
    class Program
    {
        static private IDisposable app;
        static void Main(string[] args)
        {

            int httpPort = IpPorts.TcpPort;
            var url = string.Format("http://localhost:{0}/", httpPort);
            app = WebApp.Start(url);
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }

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
