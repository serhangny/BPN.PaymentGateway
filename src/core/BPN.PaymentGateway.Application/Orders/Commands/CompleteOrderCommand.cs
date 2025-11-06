using MediatR;

using BPN.PaymentGateway.Application.Common.Models;

namespace BPN.PaymentGateway.Application.Orders.Commands;

/// <summary>
/// Completes order
/// </summary>
public class CompleteOrderCommand : IRequest<BaseResponse<Unit>>
{
    public string OrderId { get; set; } = string.Empty;
}