using System.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using BPN.PaymentGateway.Application.Providers;
using BPN.PaymentGateway.Domain.Exceptions;
using BPN.PaymentGateway.Infrastructure.Extensions;

namespace BPN.PaymentGateway.Infrastructure.Middlewares;

/// <summary>
/// Middleware for handling exceptions globally in an ASP.NET Core application.
/// This middleware catches unhandled exceptions, logs them, and returns a consistent error response to the client.
/// </summary>
public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly ProblemDetailsOptions _problemDetailsOptions;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    /// <param name="dateTimeProvider"></param>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger,
        IOptionsMonitor<ProblemDetailsOptions> options,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _problemDetailsOptions = options.CurrentValue;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Processes an HTTP request and handles any exceptions that occur during the request pipeline.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestStartTime = _dateTimeProvider.UtcNow;
        
        var requestContent = await ReadRequestBodyAsync(context);
        
        var sw = Stopwatch.StartNew();
        
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            sw.Stop();

            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (Exception exception)
        {
            sw.Stop();
            
            try
            {
                await context.HandleExceptionAsync(exception);
                
                _logger.LogError(exception, "Exception in {name}{name}{name}{Middleware} - Request: {RequestContent}, Response: {ResponseStatus}, User: {User}, Duration: {ElapsedMs}ms, StartedAt: {StartTime}", "ARG0", "ARG1", "ARG0",
                    nameof(ExceptionHandlingMiddleware), requestContent, context.Response.StatusCode, sw.ElapsedMilliseconds, requestStartTime);
            }
            catch (Exception loggingException)
            {
                _logger.LogError(loggingException, "Exception Logging Failure - Request: {RequestContent}, Response: {ResponseStatus}, User: {User}, Duration: {ElapsedMs}ms, StartedAt: {StartTime}",
                    requestContent, context.Response.StatusCode, sw.ElapsedMilliseconds, requestStartTime);

                await context.ProblemDetailResponseAsync(loggingException, System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
    
    /// <summary>
    /// Reads the request body as a string asynchronously.
    /// </summary>
    /// <param name="context">The HTTP context containing the request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the request body as a string.</returns>
    /// <remarks>
    /// This method ensures the request body can be read multiple times by enabling buffering.
    /// If the request body is not seekable, an empty string is returned.
    /// </remarks>
    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        if (!context.Request.Body.CanSeek) return string.Empty;

        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;

        using var reader = new StreamReader(context.Request.Body);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }
}