using BPN.PaymentGateway.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace BPN.PaymentGateway.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for configuring logging in the application.
/// </summary>
public static class LoggingHostingExtensions
{
    /// <summary>
    /// Configures Serilog logging for the application using settings from the configuration file.
    /// </summary>
    /// <param name="hostBuilder">The host builder instance.</param>
    /// <returns>The updated <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
        => hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            var options = context.Configuration.GetSection("Logging").Get<LoggingOptions>();

            if (!Enum.TryParse<LogEventLevel>(options.LogLevel.Default, true, out var level))
            {
                level = LogEventLevel.Information;
            }

            if (!Enum.TryParse<LogEventLevel>(options.LogLevel.Microsoft, true, out var mlevel))
            {
                mlevel = LogEventLevel.Warning;
            }

            if (!Enum.TryParse<LogEventLevel>(options.LogLevel.System, true, out var slevel))
            {
                slevel = LogEventLevel.Error;
            }

            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Is(level)
                .MinimumLevel.Override("Microsoft", mlevel)
                .MinimumLevel.Override("System", slevel);

            if (options.ConsoleEnabled)
            {
                loggerConfiguration
                    .WriteTo.Console(new RenderedCompactJsonFormatter());
            }

            if (options.FileEnabled)
            {
                loggerConfiguration
                    .WriteTo.File(new CompactJsonFormatter(), "logs/logs", rollingInterval: RollingInterval.Day);
            }
        });
}