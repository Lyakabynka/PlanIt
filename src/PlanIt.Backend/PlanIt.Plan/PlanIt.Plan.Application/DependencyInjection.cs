using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.Storage.SQLite;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Plan.Application.Configurations;

namespace PlanIt.Plan.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

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
        
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            
            x.UsingRabbitMq((ctx, config) =>
            {
                var settings = ctx.GetRequiredService<RabbitMqConfiguration>();
                
                config.Host(new Uri(settings.Host), configurator =>
                {
                    configurator.Username(settings.Username);
                    configurator.Password(settings.Password);
                });
                
                config.ConfigureEndpoints(ctx);
            });
        });


        return services;
    }
}