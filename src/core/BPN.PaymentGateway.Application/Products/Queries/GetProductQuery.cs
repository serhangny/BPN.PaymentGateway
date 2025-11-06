using MediatR;

using BPN.PaymentGateway.Application.Common.Models;
using BPN.PaymentGateway.Application.Products.Models;

namespace BPN.PaymentGateway.Application.Products.Queries;

/// <summary>
/// Gets the products from query
/// </summary>
public class GetProductQuery : IRequest<BaseResponse<ProductListResponse>>
{
    public int Offset { get; set; } = 1;
    public int Limit { get; set; } = 10;
}