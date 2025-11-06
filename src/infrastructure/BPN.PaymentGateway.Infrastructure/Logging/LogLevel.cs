namespace BPN.PaymentGateway.Infrastructure.Logging;

/// <summary>
/// Represents log level configurations for different categories in the application.
/// </summary>
public class LogLevel
{
    /// <summary>
    /// Gets or sets the default log level for the application.
    /// </summary>
    public string Default { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the log level for system-related logs.
    /// </summary>
    public string System { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the log level for Microsoft-related logs.
    /// </summary>
    public string Microsoft { get; set; } = string.Empty;
}