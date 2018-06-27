using HamBusLib;
using HamBusLib.UdpNetwork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualRigBusWin
{
    public class ReportingThread
    {
        private UdpClient udpClient = new UdpClient();
        private static ReportingThread Instance = null;
        private Thread infoThread;
        public VirtualRigInfo rigBusDesc = VirtualRigInfo.Instance;
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public static ReportingThread GetInstance()
        {
            if (Instance == null)
                Instance = new ReportingThread();

            return Instance;
        }
        private ReportingThread() { }

        public void StartInfoThread()
        {

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            Console.WriteLine(hostName);
            // Get the IP  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            var netThread = NetworkThreadRunner.GetInstance();
            rigBusDesc = VirtualRigInfo.Instance;
            rigBusDesc.Command = "update";
            rigBusDesc.Id = Id;
            rigBusDesc.UdpPort = netThread.listenUdpPort;
            rigBusDesc.TcpPort = netThread.listenTcpPort;
            rigBusDesc.MinVersion = 1;
            rigBusDesc.MaxVersion = 1;
            rigBusDesc.host = hostName;
            rigBusDesc.ip = myIP;
            rigBusDesc.SendSyncInfo = true;
            rigBusDesc.RigType = "Virtual";
            rigBusDesc.Name = "VirtualRig";
            rigBusDesc.Type = "RigBusDesc";
            infoThread = new Thread(SendRigBusInfo);
            infoThread.Start();
        }
        public void SendRigBusInfo()
        {
            while (true)
            {
                rigBusDesc.CurrentTime = DateTime.Now;
                udpClient.Connect("255.255.255.255", Constants.DirPortUdp);
                Byte[] senddata = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(rigBusDesc));
                Console.WriteLine("sending data: {0}", rigBusDesc.CurrentTime);
                udpClient.Send(senddata, senddata.Length);
                Thread.Sleep(3000);
            }
        }
    }
}

