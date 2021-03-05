using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MasstransitServiceProvider.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MasstransitServiceProvider.Filters
{
    public class CustomPublishFilter<T> : IFilter<PublishContext<T>> 
        where T : class
    {
        public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
        {
            var serviceProvider = GetServiceProvider(context);
            if (serviceProvider == null)
            {
                throw new Exception("Unable to get ServiceProvider");
            }

            var service = serviceProvider.GetRequiredService<ServiceWithContextInfo>();
            
            context.Headers.Set(nameof(ServiceWithContextInfo), service.ContextData, true);

            await next.Send(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope(nameof(CustomPublishFilter<T>));
        }

        private IServiceProvider GetServiceProvider(PublishContext<T> context)
        {
            if (context.TryGetPayload<IServiceProvider>(out var provider))
            {
                return provider;
            }

            if (context.TryGetPayload<ConsumeContext>(out var consumeContext))
            {
                if (consumeContext.TryGetPayload<IServiceProvider>(out var newProvider))
                {
                    return newProvider;
                }
            }

            return null;
        }
    }
}