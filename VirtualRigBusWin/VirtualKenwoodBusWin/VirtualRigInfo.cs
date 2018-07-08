using HamBusLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
