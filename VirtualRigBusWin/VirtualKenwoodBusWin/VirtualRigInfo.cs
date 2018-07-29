using HamBusLib;
using HamBusLib.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKenwoodBusWin
{
    public class VirtualRigInfo : RigBusInfo
    {
        private static VirtualRigInfo instance = null;
        public static VirtualRigInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VirtualRigInfo();
                }
                return instance;
            }
        }
        public VirtualBusConf GetVirtualRigConfig(string commPort)
        {
            var virtBusConf = new VirtualBusConf();
            virtBusConf.Host = Dns.GetHostName();
            var commPortConf = new CommPortConf();

            // todo replace with call to db
            commPortConf.DisplayName = "N3FJP Logger";
            commPortConf.BaudRate = 57600;
            commPortConf.PortName = "com20";
            commPortConf.Parity = Parity.None.ToString();
            commPortConf.DataBits = 8;
            commPortConf.Handshake = "none";
            commPortConf.StopBits = StopBits.One.ToString();
            commPortConf.ReadTimeout = 5000;
            commPortConf.WriteTimeout = 500;
            virtBusConf.CommPorts.Add(commPortConf);

            commPortConf = new CommPortConf();

            // todo replace with call to db
            commPortConf.DisplayName = "ACLog Logger";
            commPortConf.BaudRate = 57600;
            commPortConf.PortName = "com19";
            commPortConf.Parity = Parity.None.ToString();
            commPortConf.DataBits = 8;
            commPortConf.Handshake = "none";
            commPortConf.StopBits = StopBits.One.ToString();
            commPortConf.ReadTimeout = 5000;
            commPortConf.WriteTimeout = 500;
            virtBusConf.CommPorts.Add(commPortConf);

            commPortConf = new CommPortConf();

            // todo replace with call to db
            commPortConf.DisplayName = "xyz Logger";
            commPortConf.BaudRate = 57600;
            commPortConf.PortName = "com18";
            commPortConf.Parity = Parity.None.ToString();
            commPortConf.DataBits = 8;
            commPortConf.Handshake = "none";
            commPortConf.StopBits = StopBits.One.ToString();
            commPortConf.ReadTimeout = 5000;
            commPortConf.WriteTimeout = 500;
            virtBusConf.CommPorts.Add(commPortConf);

            return virtBusConf;

        }
    }
}
