using Asp.Versioning;

using BPN.PaymentGateway.Infrastructure.Middlewares;

namespace BPN.PaymentGateway.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring services in the application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures API versioning for the application.
    /// <para>
    /// This method sets the default API version to 1.0, assumes the default version when not specified, 
    /// and reports the available API versions in the response. It also configures the API version readers 
    /// to support versioning through URL segments, headers, and query strings.
    /// </para>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
    internal static IServiceCollection AddApiVersioningWithVersionReader(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(x =>
        {
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.DefaultApiVersion = ApiVersion.Default;
            x.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("version"),
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver")
            );
        }).AddApiExplorer(x =>
        {
            x.GroupNameFormat = "'v'V";
            x.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    /// <summary>
    /// Adds middleware services to the application's dependency injection container.
    /// <para>
    /// This method registers middlewares in the required order:
    /// 1. <see cref="CorrelationIdMiddleware"/>: Handles generation and retrieval of correlation IDs.
    /// 2. <see cref="ExceptionHandlingMiddleware"/>: Catches exceptions, logs them, and sends error responses.
    /// </para>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance to which the middleware services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
    internal static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<CorrelationIdMiddleware>(); // Register CorrelationIdMiddleware for tracking request IDs
        services
            .AddTransient<
                ExceptionHandlingMiddleware>(); // Register ExceptionHandlingMiddleware for global exception handling

        return services;
    }
}