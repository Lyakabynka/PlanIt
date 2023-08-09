using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.RabbitMq;
using PlanIt.Worker.Application.Configurations;

namespace PlanIt.Worker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
        
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ScheduledPlanTriggeredConsumer>();

            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, config) =>
            {
                var settings = context.GetRequiredService<RabbitMqSetupConfiguration>();
                var queueSettings = context.GetRequiredService<RabbitMqQueuesConfiguration>();
                
                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });
                
                config.ReceiveEndpoint(queueSettings.ScheduledPlanTriggered, ep =>
                {
                    ep.ConfigureConsumer<ScheduledPlanTriggeredConsumer>(context);
                });
            });
        });


        return services;
    }
}