using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

using BPN.PaymentGateway.Application.Orders.Commands;

namespace BPN.PaymentGateway.Api.Controllers.v1;

/// <summary>
/// Handles orders operations such as retrieval, creation, and management of products.
/// </summary>
/// <remarks>
/// Inherits from <see cref="BaseApiController"/> to apply common API behaviors and routing conventions.
/// </remarks>
[ApiVersion(1)]
[Route("v{version:apiVersion}/api/[controller]")]
public class OrdersController : BaseApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var result = await Mediator.Send(command);
        
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }
    
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteOrder([FromRoute] string id)
    {
        var orderId = id;
        
        var result = await Mediator.Send(new CompleteOrderCommand { OrderId = orderId });
        
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }
}