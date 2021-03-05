using System;
using System.Threading.Tasks;
using MassTransit;
using MasstransitServiceProvider.Commands;
using MasstransitServiceProvider.Services;

namespace MasstransitServiceProvider.Consumers
{
    public class ResultCommandConsumer : IConsumer<ResultCommand>
    {
        private readonly ServiceWithContextInfo _serviceWithContextInfo;

        public ResultCommandConsumer(ServiceWithContextInfo serviceWithContextInfo)
        {
            _serviceWithContextInfo = serviceWithContextInfo;
        }
        
        public Task Consume(ConsumeContext<ResultCommand> context)
        {
            if (_serviceWithContextInfo.ContextData != "ChangedContextData")
            {
                throw new Exception("ContextData is unchanged");
            }
            
            return Task.CompletedTask;
        }
    }
}