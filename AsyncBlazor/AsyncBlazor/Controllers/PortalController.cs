using Microsoft.Extensions.DependencyInjection;
using HorseBarn.lib;
using Microsoft.AspNetCore.Mvc;
using Neatoo.Portal;
using Neatoo.Portal.Core;
using System.Diagnostics;
using System.Reflection;

namespace HorseBarn.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortalController : ControllerBase
    {

        private readonly ILogger<PortalController> _logger;
        private readonly ServerHandlePortalRequest handlePortalRequestDelegate;

        public PortalController(ILogger<PortalController> logger, ServerHandlePortalRequest handlePortalRequestDelegate)
        {
            _logger = logger;
            this.handlePortalRequestDelegate = handlePortalRequestDelegate;
        }

        [HttpPost]
        public async Task<PortalResponse> Post(PortalRequest portalRequest)
        {
            return await handlePortalRequestDelegate(portalRequest);
        }
    }
}
