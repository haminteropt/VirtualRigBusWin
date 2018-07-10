using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace VirtualKenwoodBusWin.Controller
{

    public class SerialController : ApiController
    {
        [HttpGet]
        [Route("api/Serial/V1/portlist")]
        public string[] Get()
        {
            var portList = SerialPort.GetPortNames();
            return portList;
        }
    }
}
