using System.Net.Mime;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using Hangfire;
using Hangfire.Storage.SQLite;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Messaging;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Features.Behaviors;
using PlanIt.Plan.Application.Features.Interfaces;

namespace PlanIt.Plan.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ScheduledPlanProcessedConsumer>();
            x.AddConsumer<ScheduledPlanGroupProcessedConsumer>();

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

                config.ReceiveEndpoint(queueSettings.ScheduledPlanProcessed,
                    ep => { ep.ConfigureConsumer<ScheduledPlanProcessedConsumer>(context); });
                config.ReceiveEndpoint(queueSettings.ScheduledPlanGroupProcessed,
                    ep => { ep.ConfigureConsumer<ScheduledPlanGroupProcessedConsumer>(context); });

                EndpointConvention.Map<ScheduledPlanTriggered>(
                    new Uri($"queue:{queueSettings.ScheduledPlanTriggered}"));
                EndpointConvention.Map<ScheduledPlanGroupTriggered>(
                    new Uri($"queue:{queueSettings.ScheduledPlanGroupTriggered}"));
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