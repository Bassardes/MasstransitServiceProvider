using MassTransit;
using MassTransit.Definition;
using MasstransitServiceProvider.Consumers;
using MasstransitServiceProvider.Filters;
using MasstransitServiceProvider.Properties;
using MasstransitServiceProvider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasstransitServiceProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var rabbitConfig = Configuration.GetSection("RabbitConfig").Get<RabbitConfig>();

            services.AddScoped<ServiceForPublish>();
            services.AddScoped<ServiceWithContextInfo>();
            services.AddScoped<CommandWithPublishFromServiceConsumer>();
            services.AddScoped<CommandPublishFromContextConsumer>();
            services.AddScoped<ResultCommandConsumer>();
            services.AddScoped(typeof(CustomPublishFilter<>));
            services.AddScoped(typeof(CustomConsumeFilter<>));

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<CommandWithPublishFromServiceConsumer>();
                x.AddConsumer<CommandPublishFromContextConsumer>();
                x.AddConsumer<ResultCommandConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitConfig.Url, h =>
                    {
                        h.Username(rabbitConfig.UserName);
                        h.Password(rabbitConfig.Password);
                        h.PublisherConfirmation = true;
                    });

                    cfg.ConfigureEndpoints(ctx, KebabCaseEndpointNameFormatter.Instance);
                    cfg.UseConsumeFilter(typeof(CustomConsumeFilter<>), ctx);
                    cfg.UsePublishFilter(typeof(CustomPublishFilter<>), ctx);
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
