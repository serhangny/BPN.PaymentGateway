using System.Text.Json.Serialization;

namespace BPN.PaymentGateway.Application.Balances.Models;

/// <summary>
/// Represents the root API response for a simple query of a user's current balance.
/// </summary>
public class BalanceResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// The container for the user's current balance details.
    /// </summary>
    [JsonPropertyName("data")]
    public BalanceDetails Data { get; set; } = new();
}

/// <summary>
/// Details of the user's current financial balances.
/// </summary>
public class BalanceDetails
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The total funds in the user's account (available + blocked). Using decimal for precision.
    /// </summary>
    [JsonPropertyName("totalBalance")]
    public decimal TotalBalance { get; set; }

    /// <summary>
    /// The portion of the balance immediately available for new transactions.
    /// </summary>
    [JsonPropertyName("availableBalance")]
    public decimal AvailableBalance { get; set; }

    /// <summary>
    /// The funds currently held (blocked) by pre-orders or pending transactions.
    /// </summary>
    [JsonPropertyName("blockedBalance")]
    public decimal BlockedBalance { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp when the balance was last updated.
    /// </summary>
    [JsonPropertyName("lastUpdated")]
    public DateTimeOffset LastUpdated { get; set; }
}