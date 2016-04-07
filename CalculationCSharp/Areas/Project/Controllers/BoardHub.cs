using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CalculationCSharp.Areas.Project.Controllers
{
    [HubName("KanbanBoard")]
    public class BoardHub : Hub
    {
        public void NotifyBoardUpdated()
        {
            Clients.All.BoardUpdated();
        }
    }
}