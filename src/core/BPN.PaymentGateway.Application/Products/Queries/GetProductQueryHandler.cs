using MediatR;

using BPN.PaymentGateway.Application.Common.Models;
using BPN.PaymentGateway.Application.Products.Models;
using BPN.PaymentGateway.Application.Clients;

namespace BPN.PaymentGateway.Application.Products.Queries;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, BaseResponse<ProductListResponse>>
{
    private readonly IBalanceManagementClient _balanceManagementClient;
    
    /// <summary>
    /// CTOR
    /// </summary>
    public GetProductQueryHandler(IBalanceManagementClient balanceManagementClient)
    {
        _balanceManagementClient = balanceManagementClient;
    }
    
    /// <summary>
    /// Handles the query
    /// </summary>
    public async Task<BaseResponse<ProductListResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var response =  await _balanceManagementClient.GetProductsAsync(cancellationToken);

        if (response == null)
        {
            return BaseResponse<ProductListResponse>.Failure();
        }

        return BaseResponse<ProductListResponse>.Success(response);
    }
}