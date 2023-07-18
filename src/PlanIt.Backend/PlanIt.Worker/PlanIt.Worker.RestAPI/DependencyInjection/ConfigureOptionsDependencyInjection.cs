using Microsoft.Extensions.Options;
using PlanIt.Worker.Application.Configurations;

namespace PlanIt.Worker.RestAPI.DependencyInjection;

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
        // services.Configure<HangfireConfiguration>(
        //     configuration.GetRequiredSection(HangfireConfiguration.HangfireSection));
        // services.AddSingleton(resolver =>
        //     resolver.GetRequiredService<IOptions<HangfireConfiguration>>().Value);
        //
        services.Configure<RabbitMqConfiguration>(
            configuration.GetRequiredSection(RabbitMqConfiguration.RabbitMqSection));
        services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value);
        //
        
        services.ConfigureOptions<ConfigureJwtBearerOptions>();
        //
        
        return services;
    }
}