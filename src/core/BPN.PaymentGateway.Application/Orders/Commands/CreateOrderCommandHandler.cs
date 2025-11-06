using MediatR;
using Microsoft.Extensions.Caching.Memory;

using BPN.PaymentGateway.Application.Common.Models;
using BPN.PaymentGateway.Application.Clients;

namespace BPN.PaymentGateway.Application.Orders.Commands;

/// <summary>
/// Handles the create order command
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<int>>
{
    //private readonly IMemoryCache _memoryCache;
    private readonly IBalanceManagementClient _balanceManagementClient;


    /// <summary>
    /// Using as persistence
    /// </summary>
    public CreateOrderCommandHandler(IBalanceManagementClient balanceManagementClient)
    {
        //_memoryCache = memoryCache;
        _balanceManagementClient = balanceManagementClient;
    }

    /// <summary>
    /// Handler
    /// </summary>
    public async Task<BaseResponse<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response =  await _balanceManagementClient.CreatePreorderAsync(request);

        if (response == null)
        {
            return BaseResponse<int>.Failure();
        }

        return BaseResponse<int>.Success(Convert.ToInt32(response.Data.PreOrder.OrderId));
    }
}