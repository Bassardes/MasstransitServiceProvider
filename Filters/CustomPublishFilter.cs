using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MasstransitServiceProvider.Services;

namespace MasstransitServiceProvider.Filters
{
    public class CustomPublishFilter<T> : IFilter<PublishContext<T>> 
        where T : class
    {
        private readonly ServiceWithContextInfo _service;

        public CustomPublishFilter(ServiceWithContextInfo service)
        {
            _service = service;
        }
        
        public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
        {
            context.Headers.Set(nameof(ServiceWithContextInfo), _service.ContextData, true);

            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(CustomPublishFilter<T>));
        }
    }
}