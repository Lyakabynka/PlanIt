using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.Storage.SQLite;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Plan.Application.Configurations;
using PlanIt.Plan.Application.Hubs.Helpers;

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

        services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.AddScoped<PlanHubHelper>();
        
        return services;
    }
}