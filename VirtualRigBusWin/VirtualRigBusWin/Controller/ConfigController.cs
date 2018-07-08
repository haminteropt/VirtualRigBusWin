using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace VirtualRigBusWin.Controller
{
    class ConfigController : ApiController
    {
        // GET api/rig 
        [Route("api/RigBus/V1/RigsInfo")]
        public string Get()
        {

            return "rigState";
        }
    }
}
