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
            x.AddConsumer<InstantPlanTriggeredConsumer>();
            x.AddConsumer<OneOffPlanTriggeredConsumer>();
            x.AddConsumer<RecurringPlanTriggeredConsumer>();
            
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
                
                
                config.ReceiveEndpoint(queueSettings.InstantPlanTriggered, ep =>
                {
                    ep.ConfigureConsumer<InstantPlanTriggeredConsumer>(context);
                });
                
                config.ReceiveEndpoint(queueSettings.OneOffPlanTriggered, ep =>
                {
                    ep.ConfigureConsumer<OneOffPlanTriggeredConsumer>(context);
                });
                
                config.ReceiveEndpoint(queueSettings.RecurringPlanTriggered, ep =>
                {
                    ep.ConfigureConsumer<RecurringPlanTriggeredConsumer>(context);
                });
            });
        });


        return services;
    }
}