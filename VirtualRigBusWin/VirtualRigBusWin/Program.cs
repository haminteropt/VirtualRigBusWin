using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamBusLib.UdpNetwork;
using KenwoodEmulator;


namespace VirtualRigBusWin
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkThreadRunner.GetInstance();
            var netThread = NetworkThread.GetInstance();
            netThread.StartInfoThread();
            var kenwood = new KenwoodEmu();
            kenwood.OpenPort("com20");
            Console.ReadKey();
        }
    }
}
