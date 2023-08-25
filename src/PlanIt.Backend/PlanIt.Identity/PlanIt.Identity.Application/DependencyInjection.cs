using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Identity.Application.Features.Behaviors;
using PlanIt.Identity.Application.Services;

namespace PlanIt.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<JwtProvider>();
        services.AddScoped<CookieProvider>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        //TODO: services.AddSignalR();

        return services;
    }
}