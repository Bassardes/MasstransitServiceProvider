using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MasstransitServiceProvider.Services;

namespace MasstransitServiceProvider.Filters
{
    public class CustomConsumeFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        private readonly ServiceWithContextInfo _service;

        public CustomConsumeFilter(ServiceWithContextInfo service)
        {
            _service = service;
        }
        
        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            _service.ContextData = context.Headers.Get<string>(nameof(ServiceWithContextInfo));

            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(CustomConsumeFilter<T>));
        }
    }
}