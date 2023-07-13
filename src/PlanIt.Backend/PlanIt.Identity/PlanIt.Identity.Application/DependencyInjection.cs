using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<JwtProvider>();
        services.AddScoped<CookieProvider>();

        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        //TODO: services.AddSignalR();
        
        return services;
    }
}
