using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HamBusLib.Models.Configuration;

namespace VirtualKenwoodBusWin.Controller
{

    public class SerialController : ApiController
    {
        [HttpGet]
        [Route("api/Serial/V1/portlist")]
        public string[] GetSerialPorts()
        {
            var portList = SerialPort.GetPortNames();
            return portList;
        }

        [HttpGet]
        [Route("api/Serial/V1/portConfiguration")]
        public List<CommPortConf> GetPortConfig()
        {
            var serialConfig = VirtualRigInfo.Instance.GetVirtualRigConfig();

            return serialConfig.CommPorts;
        }
        [HttpGet]
        [Route("api/Serial/V1/portConfiguration/{port}")]
        public CommPortConf GetPortConfig(string port)
        {
            var serialConfig = VirtualRigInfo.Instance.GetVirtualRigConfig();
            var conf = serialConfig.CommPorts.Find(portList =>
            {
                if (portList.PortName.ToLower() == port.ToLower()) return true;
                else return false;
            });


            return conf;
        }
    }
}
