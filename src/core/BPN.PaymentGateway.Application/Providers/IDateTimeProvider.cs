namespace BPN.PaymentGateway.Application.Providers;

/// <summary>
/// Provides an abstraction for retrieving the current date and time with additional utilities.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    DateTime Now { get; }
    
    /// <summary>
    /// Gets the current UTC time in ISO 8601 format (e.g., "2025-02-04T12:34:56Z").
    /// </summary>
    string UtcIso8601 { get; }

    /// <summary>
    /// Gets the current local time in ISO 8601 format (e.g., "2025-02-04T12:34:56+03:00").
    /// </summary>
    string Iso8601 { get; }
    
    /// <summary>
    /// Gets the current date and time in the specified time zone.
    /// </summary>
    /// <param name="timeZoneId">The IANA or Windows time zone ID (e.g., "America/New_York" or "Eastern Standard Time").</param>
    /// <returns>The current date and time in the specified time zone.</returns>
    DateTime GetCurrentTimeInTimeZone(string timeZoneId);
}