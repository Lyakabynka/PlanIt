using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Messaging;
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
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


        services.AddMassTransit(x =>
        {
            x.AddConsumer<ScheduledPlanTriggeredConsumer>();
            x.AddConsumer<ScheduledPlanGroupTriggeredConsumer>();

            x.UsingRabbitMq((context, config) =>
            {
                var settings = context.GetRequiredService<RabbitMqSetupConfiguration>();
                var queueSettings = context.GetRequiredService<RabbitMqQueuesConfiguration>();

                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });

                config.UseJsonDeserializer(true);
                config.UseJsonSerializer();
                config.ConfigureJsonSerializerOptions(options =>
                {
                    options.Converters.Add(new JsonStringEnumConverter());
                    return options;
                });

                config.ReceiveEndpoint(queueSettings.ScheduledPlanTriggered,
                    ep => { ep.ConfigureConsumer<ScheduledPlanTriggeredConsumer>(context); });
                config.ReceiveEndpoint(queueSettings.ScheduledPlanGroupTriggered,
                    ep => { ep.ConfigureConsumer<ScheduledPlanGroupTriggeredConsumer>(context); });

                EndpointConvention.Map<ScheduledPlanProcessed>(
                    new Uri($"queue:{queueSettings.ScheduledPlanProcessed}"));
                EndpointConvention.Map<ScheduledPlanGroupProcessed>(
                    new Uri($"queue:{queueSettings.ScheduledPlanGroupProcessed}"));
            });
        });


        return services;
    }
}