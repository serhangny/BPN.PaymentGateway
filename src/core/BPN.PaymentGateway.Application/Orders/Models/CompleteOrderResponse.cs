using System.Text.Json.Serialization;

namespace BPN.PaymentGateway.Application.Orders.Models;

public class CompleteOrderResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The container for the transactional details, including order specifics and updated balances.
    /// </summary>
    [JsonPropertyName("data")]
    public OrderTransactionData Data { get; set; } = new OrderTransactionData();
}

/// <summary>
/// Contains the data payload for the order transaction, grouping order details and balance updates.
/// </summary>
public class OrderTransactionData
{
    [JsonPropertyName("order")]
    public Order Order { get; set; } = new Order();

    [JsonPropertyName("updatedBalance")]
    public UpdatedBalance UpdatedBalance { get; set; } = new UpdatedBalance();
}

/// <summary>
/// Details regarding the specific order associated with the transaction.
/// </summary>
public class Order
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// The monetary amount associated with the order. Using decimal for precision.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// The initial timestamp of the order event.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp when the order was successfully completed. Nullable if not completed.
    /// </summary>
    [JsonPropertyName("completedAt")]
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// The timestamp when the order was cancelled. Nullable if not cancelled.
    /// </summary>
    [JsonPropertyName("cancelledAt")]
    public DateTimeOffset? CancelledAt { get; set; }
}
