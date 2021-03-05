﻿using System.Threading.Tasks;
using MassTransit;
using MasstransitServiceProvider.Commands;

namespace MasstransitServiceProvider.Services
{
    public class ServiceForPublish
    {
        private readonly ServiceWithContextInfo service;
        private readonly IPublishEndpoint _publishEndpoint;

        public ServiceForPublish(
            IPublishEndpoint publishEndpoint,
            ServiceWithContextInfo service)
        {
            _publishEndpoint = publishEndpoint;
            this.service = service;
        }

        public async Task PublishCommand()
        {
            service.ContextData = "ChangedContextData";
            await _publishEndpoint.Publish(new ResultCommand
            {
                Data = 2
            });
        }
    }
}