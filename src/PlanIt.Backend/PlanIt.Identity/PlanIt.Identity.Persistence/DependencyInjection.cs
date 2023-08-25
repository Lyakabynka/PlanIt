using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlanIt.Identity.Application.Configurations;
using PlanIt.Identity.Application.Features.Interfaces;

namespace PlanIt.Identity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Get service configuration from services
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbConfig = scope.ServiceProvider.GetRequiredService<DatabaseConfiguration>();

        // register db context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string filledConnectionString;
            // Development :
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Docker.Development")
            {
                filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);
                options.UseNpgsql(filledConnectionString);
            }
            else if(environment == "Development")
            {
                filledConnectionString = dbConfig.ConnectionString;
                options.UseSqlite(filledConnectionString);
            }
            else
            {
                throw new Exception("Failed on creating connection string. Invalid environment.");
            }
            // Docker: filledConnectionString = string.Format(dbConfig.ConnectionString, dbConfig.User, dbConfig.Password);
            //options.UseNpgsql(filledConnectionString);

            // for optimizing read-only queries, disabling caching of entities
            // in ef select queries before update should be added .AsTracking method  
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        // bind db context interface to class
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}