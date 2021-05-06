using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WeatherStationWebAPI.WebSocket
{
    public class WeatherHub: Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("Update:", user, message);
        }

        public void JoinGroup(long placeid)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, placeid.ToString());
        }

        public void LeaveGroup(long placeid)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, placeid.ToString());
        }

    }
}
