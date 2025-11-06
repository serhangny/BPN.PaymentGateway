namespace BPN.PaymentGateway.Infrastructure.Logging;

/// <summary>
/// Represents configuration options for logging in the application.
/// </summary>
public class LoggingOptions
{
    /// <summary>
    /// Gets or sets the log level configuration.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether console logging is enabled.
    /// </summary>
    public bool ConsoleEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether file logging is enabled.
    /// </summary>
    public bool FileEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether HTTP request and response logging is enabled.
    /// </summary>
    public bool HttpRequestResponseType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether logging for scheduled tasks is enabled.
    /// </summary>
    public bool SchedulerType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether event bus logging is enabled.
    /// </summary>
    public bool EventbusType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether database logging is enabled.
    /// </summary>
    public bool DatabaseType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether processor logging is enabled.
    /// </summary>
    public bool ProcessorType { get; set; }
}