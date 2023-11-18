
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SandboxController: ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IHubContext<SandboxHub, ISandboxHub> hubContext;

        public SandboxController(IMemoryCache memoryCache, IHubContext<SandboxHub, ISandboxHub> hubContext)
        {
            this.memoryCache = memoryCache;
            this.hubContext = hubContext;
        }


        [HttpGet]
        [Route("Get/{sessionId}")]
        public IActionResult GetJson(string sessionId, int version)
        {
            if (memoryCache.TryGetValue(sessionId, out var result)) {
                return Ok(result);
            }
            
            return BadRequest();
        }

        [HttpPost]
        [Route("Store")]
        public IActionResult StoreJson(Session session)
        {
            memoryCache.Set(session.SessionId, session);
            hubContext.Clients.Group(session.SessionId).VersionUpdated(session.Version);
            return Ok();
        }
    }

    public class Session {
        public int Version { get; set; }
        public string SessionId { get; set; }
        public object Content { get; set; }

    }
}
