using MediatR;

using BPN.PaymentGateway.Application.Common.Models;

namespace BPN.PaymentGateway.Application.Orders.Commands;

/// <summary>
/// Creates order
/// </summary>
public class CreateOrderCommand : IRequest<BaseResponse<Unit>>
{
    public string OrderId { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
}