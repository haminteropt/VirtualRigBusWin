using HamBusLib;
using HamBusLib.UdpNetwork;
using KenwoodEmulator;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKenwoodBusWin
{
    class Program
    {
        static void Main(string[] args)
        {

            int httpPort = IpPorts.TcpPort;
            var url = string.Format("http://localhost:{0}/", httpPort);

            var comPort = "com20";
            var udpServer = UdpServer.GetInstance();
            var reportingThread = ReportingThread.GetInstance();
            reportingThread.rigBusDesc.ComPort = comPort;
            reportingThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.Id = reportingThread.Id;

            kenwood.OpenPort(comPort);

            var kenwood2 = new KenwoodEmu();
            using (WebApp.Start<Startup>(url: url))
            {
                HttpClient client = new HttpClient();

                var response = client.GetAsync(url + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                // Create HttpCient and make a request to api/values 
                Console.ReadLine();
            }
        }
    }
}
