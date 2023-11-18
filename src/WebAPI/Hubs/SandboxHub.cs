using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class SandboxHub : Hub<ISandboxHub>
    {
        public async Task VersionUpdated(string remoteSessionId, int version)
        {            
            await Clients.Group(remoteSessionId).VersionUpdated(version);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
