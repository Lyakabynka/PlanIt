using System.Text.Json.Serialization;
using Consume;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
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
                var settings = context.GetRequiredService<RabbitMqConfiguration>();
                
                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });
                
                config.ConfigureEndpoints(context);
            });
        });


        return services;
    }
}