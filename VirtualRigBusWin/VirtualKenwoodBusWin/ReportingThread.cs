using HamBusLib;
using HamBusLib.Models;
using HamBusLib.Packets;
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
        private ReportingThread() { }

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
            rigBusDesc.DocType = DocTypes.RigBusInfo; ;

            infoThread = new Thread(SendRigBusInfo);
            infoThread.Start();

            var dirClient = DirectoryClient.Instance;
            dirClient.StartThread();
        }
        public void SendRigBusInfo()
        {
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            DirGreetingList dirList = DirGreetingList.Instance;
            udpClient.EnableBroadcast = true;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);

            while (true)
            {
                try
                {
                    rigBusDesc.Time = DateTimeUtils.ConvertToUnixTime(DateTime.Now);
                    Byte[] senddata = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(rigBusDesc));


                    udpClient.Send(senddata, senddata.Length, new IPEndPoint(IPAddress.Broadcast, 7300));
                    var ServerResponseData = udpClient.Receive(ref ServerEp);
                    var ServerResponse = Encoding.ASCII.GetString(ServerResponseData);

                    var dirService = DirectoryBusGreeting.ParseCommand(ServerResponse);
                    DirGreetingList.Instance.Add(dirService);

                    Thread.Sleep(HamBusEnv.SleepTimeMs);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Timeout: Maybe DirectoryBus isn't running! ");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Timeout: Maybe DirectoryBus isn't running! \nExceptions: {0}", e.ToString());
                }
            }
        }
    }
}

