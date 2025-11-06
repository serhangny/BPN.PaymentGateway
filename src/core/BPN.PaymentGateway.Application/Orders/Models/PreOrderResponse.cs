using System.Text.Json.Serialization;

namespace BPN.PaymentGateway.Application.Orders.Models;

/// <summary>
/// Pre order response
/// </summary>
public class PreOrderResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The container for the transactional details, including pre-order information and updated balances.
    /// </summary>
    [JsonPropertyName("data")]
    public TransactionData Data { get; set; } = new TransactionData();
}

/// <summary>
/// Contains the data payload for the transaction, grouping pre-order details and balance updates.
/// </summary>
public class TransactionData
{
    [JsonPropertyName("preOrder")]
    public PreOrder PreOrder { get; set; } = new PreOrder();

    [JsonPropertyName("updatedBalance")]
    public UpdatedBalance UpdatedBalance { get; set; } = new UpdatedBalance();
}

/// <summary>
/// Details regarding the pre-order transaction that initiated the balance change.
/// </summary>
public class PreOrder
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// The amount that was pre-authorized or blocked for the order.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// The timestamp of the pre-order event, using DateTimeOffset for UTC adherence.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// The user's balance information immediately following the transaction.
/// </summary>
public class UpdatedBalance
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The total funds in the user's account. Using decimal for large monetary values.
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