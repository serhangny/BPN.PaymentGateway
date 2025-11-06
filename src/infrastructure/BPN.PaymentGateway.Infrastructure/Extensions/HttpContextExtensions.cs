using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Context;

using BPN.PaymentGateway.Application.Common.Exceptions;

namespace BPN.PaymentGateway.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for handling exceptions and generating standardized HTTP responses.
/// </summary>
internal static class HttpContextExtensions
{
    /// <summary>
    /// A dictionary mapping specific exception types to corresponding HTTP status codes.
    /// </summary>
    private static readonly Dictionary<Type, HttpStatusCode> ExceptionStatusCodes = new()
    {
        { typeof(BusinessException), HttpStatusCode.BadRequest },
        { typeof(KeyNotFoundException), HttpStatusCode.NotFound },
        { typeof(ApplicationException), HttpStatusCode.InternalServerError },
        { typeof(ValidationException), HttpStatusCode.BadRequest },
    };
    
    /// <summary>
    /// Handles an exception by determining its appropriate HTTP status code and returning a standardized problem details response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The exception that was thrown.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal static Task HandleExceptionAsync(this HttpContext context, Exception exception)
    {
        var mappedException = MapException(exception);
        var statusCode = ExceptionStatusCodes.GetValueOrDefault(mappedException.GetType(), HttpStatusCode.InternalServerError);
        return context.ProblemDetailResponseAsync(mappedException, statusCode);
    }
    
    /// <summary>
    /// Generates a problem details response with a standardized JSON format.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The exception to include in the response.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <returns>A task representing the asynchronous response writing operation.</returns>
    internal static Task ProblemDetailResponseAsync(this HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)statusCode;

        var result = JsonConvert.SerializeObject(exception, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
        });

        LogContext.PushProperty("ErrorCode", statusCode);
        return context.Response.WriteAsync(result);
    }

    /// <summary>
    /// MapException the message of an exception.
    /// </summary>
    /// <param name="ex">The exception to localize.</param>
    /// <returns>A new exception instance with a localized message.</returns>
    private static Exception MapException(Exception ex) => ex switch
    {
        BusinessException be => new BusinessException(be.ErrorCode, $"{be.ErrorCode} : {be.Message}"),
        _ => Activator.CreateInstance(ex.GetType(), ex.Message) as Exception ?? new ApplicationException(ex.Message)
    };
}