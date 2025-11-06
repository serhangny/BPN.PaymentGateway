using Microsoft.AspNetCore.Http;

namespace BPN.PaymentGateway.Infrastructure.Middlewares;

/// <summary>
/// Middleware for generating and propagating a correlation ID for each HTTP request.
/// </summary>
public class CorrelationIdMiddleware: IMiddleware
{
    internal const string CorrelationHeaderKey = "CorrelationId";

    /// <summary>
    /// Invokes the middleware to generate and propagate a correlation ID.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Check if the request already contains a correlation ID header
        if (!context.Request.Headers.ContainsKey(CorrelationHeaderKey))
        {
            // Generate a new correlation ID
            var correlationId = Guid.NewGuid().ToString();

            // Add the correlation ID to the request headers
            context.Request.Headers.Append(CorrelationHeaderKey, correlationId);
        }

        // Add the correlation ID to the response headers
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(CorrelationHeaderKey))
            {
                context.Response.Headers.Append(CorrelationHeaderKey, context.Request.Headers[CorrelationHeaderKey]);
            }
            return Task.CompletedTask;
        });

        // Call the next middleware in the pipeline
        await next(context);
    }
}