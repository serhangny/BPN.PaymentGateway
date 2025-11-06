using BPN.PaymentGateway.Application.Orders.Commands;
using BPN.PaymentGateway.Application.Orders.Models;
using BPN.PaymentGateway.Application.Products.Models;

namespace BPN.PaymentGateway.Application.Clients;

/// <summary>
/// Base interface of balance management client
/// </summary>
public interface IBalanceManagementClient
{
    Task<ProductListResponse?> GetProductsAsync(CancellationToken cancellationToken = default);
    Task<PreOrderResponse?> CreatePreorderAsync(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken = default);
    Task<CompleteOrderResponse?> CompleteOrderAsync(string orderId, CancellationToken cancellationToken = default);
}