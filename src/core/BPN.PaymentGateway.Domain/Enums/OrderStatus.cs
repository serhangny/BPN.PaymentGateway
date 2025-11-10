namespace BPN.PaymentGateway.Domain.Enums;

/// <summary>
/// Statuses of orders
/// </summary>
public enum OrderStatus
{
    Pending,
    Completed,
    Failed,
    Cancelled,
    Processing
}