using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

using BPN.PaymentGateway.Application.Products.Queries;

namespace BPN.PaymentGateway.Api.Controllers.v1;

/// <summary>
/// Handles products operations such as retrieval, creation, and management of products.
/// </summary>
/// <remarks>
/// Inherits from <see cref="BaseApiController"/> to apply common API behaviors and routing conventions.
/// </remarks>
[ApiVersion(1)]
[Route("v{version:apiVersion}/api/[controller]")]
public class ProductsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetProductQuery query)
    {
        var result = await Mediator.Send(query);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }
}