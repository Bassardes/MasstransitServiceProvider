using System.Threading.Tasks;
using MassTransit;
using MasstransitServiceProvider.Commands;
using MasstransitServiceProvider.Services;

namespace MasstransitServiceProvider.Consumers
{
    public class CommandWithPublishFromServiceConsumer : IConsumer<CommandWithPublishFromService>
    {
        private readonly ServiceForPublish _service;

        public CommandWithPublishFromServiceConsumer(ServiceForPublish service)
        {
            _service = service;
        }
        
        public async Task Consume(ConsumeContext<CommandWithPublishFromService> context)
        {
            await _service.PublishCommand();
        }
    }
}