using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpLogging; // to use httploggingfields
namespace WebApi.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCustomHttpLogging(
        this IServiceCollection services
    )
    {
        services.AddHttpLogging(options =>
        {
            options.RequestHeaders.Add("Origin");
            // add rate limiting headers so they will not be redacted
            options.RequestHeaders.Add("X-Client-Id");
            options.ResponseHeaders.Add("Retry-After");
            options.LoggingFields = HttpLoggingFields.All;
        });

        return services;
    }
    public static IServiceCollection AddCustomCors(
        this IServiceCollection services
    )
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: "Northwind.Mvc.Policy",
            policy =>
            {
                policy.WithOrigins("https://localhost:5082");
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomRateLimiting(
        this IServiceCollection services, ConfigurationManager configuration
    )
    {
        // add services to store rate limit counters and rules in memory
        services.AddMemoryCache();
        services.AddInMemoryRateLimiting();

        // load default rate limit options from appsettings.json
        services.Configure<ClientRateLimitOptions>(
            configuration.GetSection("ClientRateLimiting")
        );

        // load client specific policies from appsettings
        services.Configure<ClientRateLimitPolicies>(
            configuration.GetSection("ClientRateLimitPolicies")
        );

        // register the configuration
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return services;
    }
}