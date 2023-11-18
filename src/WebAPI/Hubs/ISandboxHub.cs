using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public interface ISandboxHub
    {
        Task AddToGroup(string groupName);
        Task VersionUpdated(int version);
        Task RemoveFromGroup(string groupName);
    }
}
