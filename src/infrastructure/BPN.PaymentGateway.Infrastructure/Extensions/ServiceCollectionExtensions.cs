using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPN.PaymentGateway.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for registering infrastructure-related services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to which dependencies will be added.</param>
    /// <param name="configuration">The application configuration for retrieving necessary settings.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> for method chaining.</returns>
    /// <remarks>
    /// - Registers <see cref="IHttpContextAccessor"/> and <see cref="IUserContextAccessor"/>.
    /// - Future enhancements may include database, repositories, and email providers.
    /// </remarks>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        
        return services;
    }
}