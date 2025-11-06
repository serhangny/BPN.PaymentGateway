using System.Diagnostics;
using System.Dynamic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;

using BPN.PaymentGateway.Infrastructure.Logging;

namespace BPN.PaymentGateway.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for structured logging using Microsoft ILogger.
/// </summary>
public static class MicrosoftLoggingExtensions
{
    /// <summary>
    /// Logs an error message along with an exception.
    /// </summary>
    public static void LogError(this ILogger log, Exception exception, string message)
    {
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(exception);

        if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

        var logObject = new Dictionary<string, object>
        {
            ["LogType"] = LogType.Exception,
            ["TraceId"] = Activity.Current?.TraceId,
            ["@Exception"] = ReconfigureException(exception)
        };

        using (log.BeginScope(logObject))
        {
            log.LogError(message);
        }
    }
    
    /// <summary>
    /// Logs an informational message with a specified log type.
    /// </summary>
    public static void LogInformation(this ILogger log, LogType logType, string message)
    {
        using (LogContext.PushProperty("LogType", logType))
        {
            log.LogInformation(message);
        }
    }

    /// <summary>
    /// Logs information about a webhook request and response.
    /// </summary>
    public static void LogInformation(
        this ILogger log,
        string requestId,
        string requestUrl,
        string requestHeaders,
        string data,
        string response,
        HttpResponseHeaders responseHeaders,
        int statusCode,
        int retryCount = 0)
    {
        ArgumentNullException.ThrowIfNull(log);
        ArgumentNullException.ThrowIfNull(responseHeaders);

        if (string.IsNullOrEmpty(requestUrl)) throw new ArgumentNullException(nameof(requestUrl));

        var sanitizedRequestBody = SanitizeSensitiveData(data);
        var sanitizedResponseBody = SanitizeSensitiveData(response);

        var logObject = new Dictionary<string, object>
        {
            ["LogType"] = LogType.Weebhook,
            ["TraceId"] = requestId ?? string.Empty,
            ["RequestPath"] = requestUrl,
            ["RequestMethod"] = HttpMethod.Post.ToString(),
            ["RequestHeaders"] = requestHeaders ?? string.Empty,
            ["RequestBody"] = sanitizedRequestBody ?? string.Empty,
            ["ResponseStatusCode"] = statusCode,
            ["ResponseHeaders"] = SerializeHeaders(responseHeaders),
            ["ResponseBody"] = sanitizedResponseBody ?? string.Empty
        };

        using (log.BeginScope(logObject))
        {
            log.LogInformation("Webhook Request Response >> try Count : {RetryCount}", retryCount);
        }
    }
    
    /// <summary>
    /// Logs an error related to a webhook request, including request details.
    /// </summary>
    public static void LogError(this ILogger log, Exception exception, string requestId, string requestUrl,
        string headers, string data, int retryCount = 0)
    {
        var requestBody = System.Text.RegularExpressions.Regex.Replace(data,
            "(\\n?\\s*\"password\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"password\":\"*****\"");
        dynamic logObject = new ExpandoObject();
        logObject.LogType = LogType.Exception;
        logObject.TraceId = requestId;
        logObject.RequestPath = requestUrl;
        logObject.RequestMethod = HttpMethod.Post;
        logObject.RequestHeaders = headers;
        logObject.RequestBody = requestBody;
        var logObjDict = logObject as IDictionary<string, object>;
        logObjDict["@Exception"] = ReconfigureException(exception);

        using (log.BeginScope(logObject as IDictionary<string, object>))
        {
            log.LogError($"Exception >> Webhook Exception try Count : {retryCount}");
        }
    }

    /// <summary>
    /// Logs details of an HTTP request and response.
    /// </summary>
    public static void LogInformation(this ILogger log, HttpRequest request, string requestBody, HttpResponse response,
        long time, string responseBody, string message, DateTime requestStartTime)
    {
        requestBody = System.Text.RegularExpressions.Regex.Replace(requestBody,
            "(\\n?\\s*\"password\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"password\":\"*****\"");
        requestBody = System.Text.RegularExpressions.Regex.Replace(requestBody,
            "(\\n?\\s*\"otp\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"otp\":\"*****\"");

        responseBody = System.Text.RegularExpressions.Regex.Replace(responseBody,
            "(\\n?\\s*\"password\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"password\":\"*****\"");
        responseBody = System.Text.RegularExpressions.Regex.Replace(responseBody,
            "(\\n?\\s*\"otp\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"otp\":\"*****\"");

        dynamic logObject = new ExpandoObject();
        logObject.LogType = LogType.HttpRequestResponse;

        logObject.RequestPath = request.Path;
        logObject.RequestMethod = request.Method;
        logObject.RequestHeaders =
            JsonConvert.SerializeObject(request.Headers?.ToDictionary(h => h.Key, h => h.Value.ToString()));
        logObject.RequestBody = requestBody;
        if (request.QueryString.HasValue)
        {
            logObject.QueryString = request.QueryString.Value;
        }

        logObject.ResponseStatusCode = response.StatusCode;
        logObject.ResponseHeaders =
            JsonConvert.SerializeObject(response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
        logObject.ResponseBody = responseBody;

        logObject.RequestStartTime = requestStartTime;
        logObject.Duration = time;

        using (var a = log.BeginScope(logObject as IDictionary<string, object>))
        {
            log.LogInformation(message);
        }
    }

    /// <summary>
    /// Logs scheduler-related information.
    /// </summary>
    public static void LogInformation(this ILogger log, long schedulerTrackerId, string message)
    {
        dynamic logObject = new ExpandoObject();
        logObject.LogType = LogType.Scheduler;
        logObject.CurrentCulture = Thread.CurrentThread.CurrentCulture.DisplayName;
        logObject.UiCurrentCulture = Thread.CurrentThread.CurrentUICulture.DisplayName;
        logObject.SchedulerTrackerId = schedulerTrackerId;
        logObject.TraceId = Activity.Current?.ParentId;

        using (log.BeginScope(logObject as IDictionary<string, object>))
        {
            log.LogInformation(message);
        }
    }
    
    /// <summary>
    /// Logs error
    /// </summary>
    public static void LogError(this ILogger log, Exception exception, HttpRequest request, string requestBody, HttpResponse response,
    string message, long time, DateTime requestStartTime)
{
    if (!string.IsNullOrEmpty(requestBody))
    {
        requestBody = System.Text.RegularExpressions.Regex.Replace(requestBody, "(\\n?\\s*\"password\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"password\":\"*****\"");
        requestBody = System.Text.RegularExpressions.Regex.Replace(requestBody, "(\\n?\\s*\"otp\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)", "\"otp\":\"*****\"");
    }                

    dynamic logObject = new ExpandoObject();
    logObject.LogType = LogType.HttpException;

    logObject.RequestPath = request.Path;
    logObject.RequestMethod = request.Method;
    logObject.RequestHeaders = JsonConvert.SerializeObject(request.Headers?.ToDictionary(h => h.Key, h => h.Value.ToString()));
    logObject.RequestBody = requestBody;
    if (request.QueryString.HasValue)
    {
        logObject.QueryString = request.QueryString.Value;
    }

    logObject.ResponseStatusCode = response.StatusCode;
    logObject.ResponseHeaders = JsonConvert.SerializeObject(response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()));


    var logObjDict = logObject as IDictionary<string, object>;
    logObjDict["@Exception"] = ReconfigureException(exception);

    logObject.RequestStartTime = requestStartTime;
    logObject.Duration = time;

    using (log.BeginScope(logObjDict))
    {
        log.LogError(message);
    }
}
    
    #region PRIVATE METHODS

    private static string SanitizeSensitiveData(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var sanitized = System.Text.RegularExpressions.Regex.Replace(
            input,
            "(\\n?\\s*\"password\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)",
            "\"password\":\"*****\"");

        return System.Text.RegularExpressions.Regex.Replace(
            sanitized,
            "(\\n?\\s*\"otp\"\\s?:\\s?\")[^\\n\"]*(\",?\\n?)",
            "\"otp\":\"*****\"");
    }

    private static string SerializeHeaders(HttpResponseHeaders headers)
    {
        try
        {
            return JsonConvert.SerializeObject(
                headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
        catch (Exception)
        {
            return "{}";
        }
    }

    private static Exception ReconfigureException(Exception ex)
    {
        if (ex as DbUpdateException != null)
        {
            var serEx = JsonConvert.SerializeObject(ex);
            ex = JsonConvert.DeserializeObject<DbUpdateException>(serEx);
        }

        return ex;
    }

    #endregion
}