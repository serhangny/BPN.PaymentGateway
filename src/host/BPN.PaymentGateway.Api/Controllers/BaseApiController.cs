using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BPN.PaymentGateway.Api.Controllers;

/// <summary>
/// Serves as a base API controller that provides common configurations for all API controllers.
/// </summary>
/// <remarks>
/// This controller applies routing conventions and API behaviors that are shared across all derived controllers.
/// </remarks>
[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}