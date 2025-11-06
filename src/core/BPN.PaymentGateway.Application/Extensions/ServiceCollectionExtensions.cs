using System.Reflection;

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using BPN.PaymentGateway.Application.Providers;

namespace BPN.PaymentGateway.Application.Extensions;

/// <summary>
/// Provides extension methods for configuring application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application-specific services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which the application services will be added.</param>
    /// <param name="configuration">The application configuration instance.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddServices()
            .AddValidations()
            .AddMediatr();
        //.AddMemoryCache();
    }
    
    /// <summary>
    /// Registers core application services such as providers.
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
    
    /// <summary>
    /// Add validators
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        return services;
    }
    
    /// <summary>
    /// Add Mediatr
    /// </summary>
    /// <param name="services">The service collection to which the services will be added.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}