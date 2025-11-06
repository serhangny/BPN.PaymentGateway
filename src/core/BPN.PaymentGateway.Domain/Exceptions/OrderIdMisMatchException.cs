using Microsoft.AspNetCore.Http;

namespace BPN.PaymentGateway.Domain.Exceptions;

/// <summary>
/// If order id mismatched this will throw
/// </summary>
public class OrderIdMisMatchException: DomainException
{
    public OrderIdMisMatchException(string orderId)
        : base($"OrderId mismatch: {orderId}.") { }
    
    public override int StatusCode => StatusCodes.Status400BadRequest;
}