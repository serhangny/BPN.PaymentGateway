namespace BPN.PaymentGateway.Application.Providers;

/// <summary>
/// Provides the current date and time using the system clock.
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public string UtcIso8601 => UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

    /// <inheritdoc />
    public string Iso8601 => Now.ToString("yyyy-MM-ddTHH:mm:sszzz");

    /// <inheritdoc />
    public DateTime GetCurrentTimeInTimeZone(string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(UtcNow, timeZone);
        }
        catch (TimeZoneNotFoundException)
        {
            throw new ArgumentException($"Invalid time zone ID: {timeZoneId}");
        }
    }
}