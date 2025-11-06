namespace BPN.PaymentGateway.Domain.Exceptions;

/// <summary>
/// Base of all domain exceptions
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }

    public abstract int StatusCode { get; }
}