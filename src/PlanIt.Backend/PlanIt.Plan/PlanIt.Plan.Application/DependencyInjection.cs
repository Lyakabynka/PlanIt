using System.Reflection;
using Hangfire;
using Hangfire.Storage.SQLite;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Interfaces;
using PlanIt.RabbitMq;

namespace PlanIt.Plan.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ScheduledPlanProcessedConsumer>();
            
            x.UsingRabbitMq((context, config) =>
            {
                var settings = context.GetRequiredService<RabbitMqSetupConfiguration>();
                var queueSettings = context.GetRequiredService<RabbitMqQueuesConfiguration>();

                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });

                config.ReceiveEndpoint(queueSettings.ScheduledPlanProcessed, ep =>
                {
                    ep.ConfigureConsumer<ScheduledPlanProcessedConsumer>(context);
                });
                
                EndpointConvention.Map<ScheduledPlanTriggered>(
                    new Uri($"queue:{queueSettings.ScheduledPlanTriggered}"));
            });
        });

        var hangfireConfig = services.BuildServiceProvider().GetRequiredService<HangfireConfiguration>();
        services.AddHangfire(
            configuration =>
            {
                configuration.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    // .UseSerializerSettings(new JsonSerializerSettings()
                    // {
                    //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    // })
                    .UseSQLiteStorage(hangfireConfig.ConnectionString);
            });

        services.AddHangfireServer();

        services.AddScoped<IPublishHelper, PublishHelper>();

        return services;
    }
}