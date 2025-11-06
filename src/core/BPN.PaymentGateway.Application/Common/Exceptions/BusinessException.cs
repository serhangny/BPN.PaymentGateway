namespace BPN.PaymentGateway.Application.Common.Exceptions;

/// <summary>
/// Represents an application-specific business exception with an associated error code.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Gets the error code associated with the business exception.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessException"/> class with a specified error code and message.
    /// </summary>
    /// <param name="errorCode">A unique identifier for the business error.</param>
    /// <param name="message">A human-readable message that describes the error.</param>
    public BusinessException(string errorCode, string message) 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}