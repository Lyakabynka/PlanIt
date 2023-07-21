﻿using System.Reflection;
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
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, config) =>
            {
                var settings = ctx.GetRequiredService<RabbitMqSetupConfiguration>();
                var queueSettings = ctx.GetRequiredService<RabbitMqQueuesConfiguration>();

                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });

                EndpointConvention.Map<InstantPlanTriggered>(
                    new Uri($"queue:{queueSettings.InstantPlanTriggered}"));
                
                EndpointConvention.Map<OneOffPlanTriggered>(
                    new Uri($"queue:{queueSettings.OneOffPlanTriggered}"));
                
                EndpointConvention.Map<RecurringPlanTriggered>(
                    new Uri($"queue:{queueSettings.RecurringPlanTriggered}"));
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