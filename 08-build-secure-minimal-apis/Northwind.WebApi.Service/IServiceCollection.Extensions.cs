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
}