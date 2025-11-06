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
        var result = await Mediator.Send(new CreateOrderCommand());
        
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
    }
    
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteOrder([FromRoute] Guid id)
    {
        // Example: send a command to your Application layer
        // var result = await _mediator.Send(new CompleteOrderCommand(id));
        //
        // return result.IsSuccess
        //     ? Ok(result.Data)
        //     : BadRequest(result.Errors);
        
        return Ok();
    }
}