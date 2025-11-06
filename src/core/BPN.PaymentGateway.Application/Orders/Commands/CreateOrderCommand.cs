using MediatR;

using BPN.PaymentGateway.Application.Common.Models;

namespace BPN.PaymentGateway.Application.Orders.Commands;

/// <summary>
/// Creates order
/// </summary>
public class CreateOrderCommand : IRequest<BaseResponse<int>>
{
    public int OrderId { get; set; }
    
    public decimal Amount { get; set; }
}