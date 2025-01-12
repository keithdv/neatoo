using Autofac;
using HorseBarn.lib;
using Microsoft.AspNetCore.Mvc;
using Neatoo.Portal;
using Neatoo.Portal.Core;
using System.Reflection;

namespace HorseBarn.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortalController : ControllerBase
    {

        private readonly ILogger<PortalController> _logger;
        private readonly Neatoo.IServiceScope lifetimeScope;
        private readonly IPortalJsonSerializer jsonSerializer;

        public PortalController(ILogger<PortalController> logger, Neatoo.IServiceScope lifetimeScope, IPortalJsonSerializer jsonSerializer)
        {
            _logger = logger;
            this.lifetimeScope = lifetimeScope;
            this.jsonSerializer = jsonSerializer;
        }

        [HttpPost]
        public async Task<PortalResponse> Post(PortalRequest portalRequest)
        {
            var t = Type.GetType(portalRequest.Type) ?? throw new Exception($"Type {portalRequest.Type} not found");

            if (t.IsInterface)
            {
                t = lifetimeScope.ConcreteType(t);
            }

            var portal = (IPortalOperationManager) lifetimeScope.Resolve(typeof(IPortalOperationManager<>).MakeGenericType(t));

            var result = await portal.HandlePortalRequest(portalRequest);

            return new PortalResponse()
            {
                ObjectJson = jsonSerializer.Serialize(result),
                Type = portalRequest.Type
            };
        }
    }
}