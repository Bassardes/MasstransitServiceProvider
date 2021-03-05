using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MasstransitServiceProvider.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MasstransitServiceProvider.Filters
{
    public class CustomConsumeFilter<T> : IFilter<ConsumeContext<T>>
        where T : class
    {
        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            var serviceProvider = context.GetPayload<IServiceProvider>();

            var service = serviceProvider.GetRequiredService<ServiceWithContextInfo>();

            service.ContextData = context.Headers.Get<string>(nameof(ServiceWithContextInfo));

            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(CustomConsumeFilter<T>));
        }
    }
}