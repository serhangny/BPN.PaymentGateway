using BPN.PaymentGateway.Infrastructure.Middlewares;

namespace BPN.PaymentGateway.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring middleware and infrastructure in an application.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures basic infrastructure for the application, including routing, Swagger, authorization, and response compression.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance, enabling method chaining.</returns>
    internal static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        builder
            .UseSwagger()
            .UseSwaggerUI()
            .UseRouting()
            .UseAuthorization()
            .UseResponseCompression()
            .UseMiddlewares();

        return builder;
    }
    
    /// <summary>
    /// Adds a series of middlewares to the application's request pipeline.
    /// <para>
    /// The order of the middlewares is important and should be as follows:
    /// 1. <see cref="CorrelationIdMiddleware"/>: Generates or retrieves a Correlation ID for tracking.
    /// 2. <see cref="ExceptionHandlingMiddleware"/>: Handles unhandled exceptions and provides a structured error response.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance, enabling method chaining.</returns>
    private static IApplicationBuilder UseMiddlewares(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CorrelationIdMiddleware>();
        builder.UseMiddleware<ExceptionHandlingMiddleware>();

        return builder;
    }
}