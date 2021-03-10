using System;
using System.Threading.Tasks;
using MassTransit;
using MasstransitServiceProvider.Commands;
using MasstransitServiceProvider.Services;

namespace MasstransitServiceProvider.Consumers
{
    public class CommandPublishFromContextConsumer : IConsumer<CommandPublishFromContext>
    {
        private readonly ServiceWithContextInfo service;
    
        public CommandPublishFromContextConsumer(ServiceWithContextInfo service)
        {
            this.service = service;
        }
    
        public async Task Consume(ConsumeContext<CommandPublishFromContext> context)
        {
            if (service.ContextData != "ContextDataFromController")
            {
                throw new Exception("ContextData not received");
            }

            service.ContextData = "ChangedContextData";
            await context.Publish(new ResultCommand
            {
                Data = 1
            });
        }
    }
}