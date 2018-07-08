using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Text;
using System.Threading.Tasks;

namespace VirtualKenwoodBusWin.Hubs
{
    public class NotificationHub
    {
        public void ServerTime()
        {
            //Clients.All.displayTime($" {DateTime.UtcNow:T}");
        }
    }
}
