namespace BPN.PaymentGateway.Application.Common.Models;

/// <summary>
/// Represents an error response with a code and a message.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error code associated with the response.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets the detailed error message.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
