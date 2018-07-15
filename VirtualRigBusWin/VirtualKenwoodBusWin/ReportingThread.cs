﻿using HamBusLib;
using HamBusLib.Models;
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

namespace VirtualKenwoodBusWin
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
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            DirGreetingList dirList = DirGreetingList.Instance;
            udpClient.EnableBroadcast = true;
            var dirClient = DirectoryClient.Instance;
            while (true)
            {
                rigBusDesc.Time = DateTimeUtils.ConvertToUnixTime(DateTime.Now);
                Byte[] senddata = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(rigBusDesc));


                udpClient.Send(senddata, senddata.Length, new IPEndPoint(IPAddress.Broadcast, 7300));
                Console.WriteLine("Sending Data");
                var ServerResponseData = udpClient.Receive(ref ServerEp);
                var ServerResponse = Encoding.ASCII.GetString(ServerResponseData);
                Console.WriteLine("Recived {0} from {1} port {2}", ServerResponse,
                    ServerEp.Address.ToString(), ServerEp.Port);
                var dirService = DirectoryBusGreeting.ParseCommand(ServerResponse);
                DirGreetingList.Instance.Add(dirService);
                dirClient.StartThread();
                Thread.Sleep(3000);
            }
        }
    }
}

