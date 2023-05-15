using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NewScotApp.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string userName = Context.User.Claims.FirstOrDefault(x => x.Type == "Name")!.Value;
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
        public async Task onConnected()
        {
            await Clients.All.SendAsync("ReceiveMessage", "A user has connected");
        }
        public async Task onDisconnected()
        {
            await Clients.All.SendAsync("ReceiveMessage", "Server", "A user has disconnected");
        }
    }

}
