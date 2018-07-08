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
        private ReportingThread()
        {

        }

        public void StartInfoThread()
        {

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();

            var udpServer = UdpServer.GetInstance();
            rigBusDesc = VirtualRigInfo.Instance;
            rigBusDesc.Command = "update";
            rigBusDesc.Id = Id;
            rigBusDesc.UdpPort = udpServer.listenUdpPort;
            rigBusDesc.TcpPort = udpServer.listenTcpPort;
            rigBusDesc.MinVersion = 1;
            rigBusDesc.MaxVersion = 1;
            rigBusDesc.Host = hostName;
            rigBusDesc.Description = "Kenwood Virtual RigBus";
            rigBusDesc.Ip = myIP;
            rigBusDesc.SendSyncInfo = true;
            rigBusDesc.RigType = "Virtual";
            rigBusDesc.Name = "VirtualRig";
            rigBusDesc.DocType = "RigBus";

            infoThread = new Thread(SendRigBusInfo);
            infoThread.Start();
        }
        public void SendRigBusInfo()
        {
            while (true)
            {
                rigBusDesc.Time = DateTimeUtils.ConvertToUnixTime(DateTime.Now);
                udpClient.Connect("255.255.255.255", Constants.DirPortUdp);
                Byte[] senddata = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(rigBusDesc));
                udpClient.Send(senddata, senddata.Length);
                Thread.Sleep(3000);
            }
        }
    }
}

