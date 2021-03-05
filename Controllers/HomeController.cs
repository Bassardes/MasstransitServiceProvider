using System.Net.Mime;
using System.Threading.Tasks;
using MassTransit;
using MasstransitServiceProvider.Commands;
using MasstransitServiceProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasstransitServiceProvider.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class HomeController : ControllerBase
    {
        private readonly ServiceWithContextInfo _serviceWithContextInfo;
        private readonly IPublishEndpoint _publishEndpoint;

        public HomeController(
            IPublishEndpoint publishEndpoint,
            ServiceWithContextInfo serviceWithContextInfo)
        {
            _publishEndpoint = publishEndpoint;
            _serviceWithContextInfo = serviceWithContextInfo;
        }

        [HttpGet]
        public async Task PublishFromService()
        {
            _serviceWithContextInfo.ContextData = "ContextDataFromController";

            await _publishEndpoint.Publish(new CommandWithPublishFromService
            {
                Data = 1
            });
        }
        
        [HttpGet]
        public async Task PublishDirectly()
        {
            _serviceWithContextInfo.ContextData = "ContextDataFromController";

            await _publishEndpoint.Publish(new CommandPublishFromContext
            {
                Data = 1
            });
        }
    }
}
