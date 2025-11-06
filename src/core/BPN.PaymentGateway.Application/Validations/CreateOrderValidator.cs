using FluentValidation;

using BPN.PaymentGateway.Application.Orders.Commands;

namespace BPN.PaymentGateway.Application.Validations;

/// <summary>
/// Custom validator for create order
/// </summary>
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");
        
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.")
            .Matches(@"^[A-Za-z0-9\-]+$").WithMessage("OrderId must be alphanumeric."); 

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}