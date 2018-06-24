using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KenwoodEmulator;


namespace VirtualRigBusWin
{
    class Program
    {
        static void Main(string[] args)
        {
            var kenwood = new KenwoodEmu();
            kenwood.OpenPort("com20");
            Console.ReadKey();
        }
    }
}
