using MediatR;
using Microsoft.Extensions.Caching.Memory;

using BPN.PaymentGateway.Application.Common.Models;
using BPN.PaymentGateway.Application.Clients;

namespace BPN.PaymentGateway.Application.Orders.Commands;

/// <summary>
/// Handles the create order command
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<Unit>>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBalanceManagementClient _balanceManagementClient;


    /// <summary>
    /// Using as persistence
    /// </summary>
    public CreateOrderCommandHandler(IBalanceManagementClient balanceManagementClient, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _balanceManagementClient = balanceManagementClient;
    }

    /// <summary>
    /// Handler
    /// </summary>
    public async Task<BaseResponse<Unit>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response =  await _balanceManagementClient.CreatePreorderAsync(request);

        if (response == null)
        {
            return BaseResponse<Unit>.Failure();
        }
        
        _memoryCache.Set(request.OrderId, response);
        
        return BaseResponse<Unit>.Success(Unit.Value);
    }
}