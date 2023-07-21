using Microsoft.Extensions.Options;
using PlanIt.Plan.Application.Configurations;

namespace PlanIt.Plan.RestAPI.DependencyInjection;

public static class ConfigureOptionsDependencyInjection
{
    public static IServiceCollection AddCustomConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfiguration>(
            configuration.GetRequiredSection(DatabaseConfiguration.DatabaseSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
        //
        services.Configure<JwtConfiguration>(
            configuration.GetRequiredSection(JwtConfiguration.JwtSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<JwtConfiguration>>().Value);
        //
        services.Configure<HangfireConfiguration>(
            configuration.GetRequiredSection(HangfireConfiguration.HangfireSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<HangfireConfiguration>>().Value);
        //
        services.Configure<RabbitMqSetupConfiguration>(
            configuration.GetRequiredSection(RabbitMqSetupConfiguration.RabbitMqSetupSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RabbitMqSetupConfiguration>>().Value);
        //
        services.Configure<RabbitMqQueuesConfiguration>(
            configuration.GetRequiredSection(RabbitMqQueuesConfiguration.RabbitMqQueuesSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RabbitMqQueuesConfiguration>>().Value);
        //
        
        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        //
        
        return services;
    }
}