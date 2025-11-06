using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using BPN.PaymentGateway.Infrastructure.Guard;

namespace BPN.PaymentGateway.Infrastructure.Extensions;

/// <summary>
/// Add to methods that check input for null and throw if the input is null.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ValidatedNotNullAttribute : Attribute { }

/// <summary>
/// Null check extensions
/// </summary>
public static class NullCheckExtensions
{
    /// <summary>
    /// Ensures that the given string is neither null nor empty.
    /// </summary>
    /// <param name="guardClause">The guard clause instance.</param>
    /// <param name="input">The input string to validate.</param>
    /// <param name="parameterName">The name of the parameter being checked (automatically populated).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <param name="exceptionCreator">Optional function to create a custom exception.</param>
    /// <returns>The validated non-null, non-empty string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input is null.</exception>
    public static string NullOrEmpty(this IGuardClause guardClause,
        [NotNull] [ValidatedNotNull] string? input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        GuardClause.Against.Null(input, parameterName, message, exceptionCreator);
        if (input == string.Empty)
        {
            throw exceptionCreator?.Invoke() ??
                  new ArgumentException(message ?? $"Required input {parameterName} was empty.", parameterName);
        }

        return input;
    }

    /// <summary>
    /// Ensures that the given input is not null.
    /// </summary>
    /// <typeparam name="T">The type of the input parameter.</typeparam>
    /// <param name="guardClause">The guard clause instance.</param>
    /// <param name="input">The input value to validate.</param>
    /// <param name="parameterName">The name of the parameter being checked (automatically populated).</param>
    /// <param name="message">Optional custom error message.</param>
    /// <param name="exceptionCreator">Optional function to create a custom exception.</param>
    /// <returns>The validated non-null object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input is null.</exception>
    public static T Null<T>(this IGuardClause guardClause, [NotNull] [ValidatedNotNull] T? input,
        [CallerArgumentExpression("input")] string? parameterName = null, string? message = null,
        Func<Exception>? exceptionCreator = null)
    {
        if (input is null)
        {
            Exception? exception = exceptionCreator?.Invoke();

            if (string.IsNullOrEmpty(message))
            {
                throw exception ?? new ArgumentNullException(parameterName);
            }

            throw exception ?? new ArgumentNullException(parameterName, message);
        }

        return input;
    }
    
    
}