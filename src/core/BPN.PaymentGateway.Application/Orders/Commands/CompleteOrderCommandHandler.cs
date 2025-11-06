using MediatR;

using Microsoft.Extensions.Caching.Memory;

using BPN.PaymentGateway.Application.Clients;
using BPN.PaymentGateway.Application.Common.Models;

namespace BPN.PaymentGateway.Application.Orders.Commands;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, BaseResponse<Unit>>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBalanceManagementClient _balanceManagementClient;

    /// <summary>
    /// CTOR
    /// </summary>
    /// <param name="memoryCache"></param>
    /// <param name="balanceManagementClient"></param>
    public CompleteOrderCommandHandler(IMemoryCache memoryCache, IBalanceManagementClient balanceManagementClient)
    {
        _memoryCache = memoryCache;
        _balanceManagementClient = balanceManagementClient;
    }

    /// <summary>
    /// Handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    public async Task<BaseResponse<Unit>> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var response =  await _balanceManagementClient.CompleteOrderAsync(request.OrderId, cancellationToken);

        if (response == null)
        {
            return BaseResponse<Unit>.Failure();
        }
        
        _memoryCache.Set(request.OrderId, response);
        
        return BaseResponse<Unit>.Success(Unit.Value);
    }
}