using Microsoft.AspNetCore.Http;

namespace BPN.PaymentGateway.Domain.Exceptions;

/// <summary>
/// For balance query exception
/// </summary>
public class InsufficientBalanceException : DomainException
{
    public InsufficientBalanceException(decimal balance)
        : base($"Insufficient balance. Available balance: {balance}.") { }

    public override int StatusCode => StatusCodes.Status400BadRequest;
}