using EquipmentService.Api.ApiRequests;
using EquipmentService.Api.Common;
using EquipmentService.Api.Dtos;
using EquipmentService.Api.Features.Equipments.Create;
using EquipmentService.Api.Features.Equipments.Get;
using EquipmentService.Api.Features.Equipments.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.ResultPattern.Errors;

namespace EquipmentService.Api.Controllers;

[ApiController]
[Route("api/equipments")]
public class EquipmentController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EquipmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task <ActionResult<PaginatedListResponse<EquipmentDto>>> ListAsync(
        [FromQuery]int limit = 10,
        [FromQuery]string? cursorToken = null,
        CancellationToken cancellationToken = default)
    {
        var request = new ListEquipmentRequest(limit, cursorToken);

        var result = await _mediator.Send(request, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            _ => BadRequest(result.Error.Message)
        };
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EquipmentDto>> GetAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new GetEquipmentRequest(id);

        var result = await _mediator.Send(request, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.Error.Message),
            _ => BadRequest(result.Error.Message)
        };
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateEquipmentApiRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateEquipmentCommand(request.Name, request.SerialNumber, request.Category, request.Voltage,
            request.DeliveredAt, request.WarrantyExpiresAt);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            ErrorCode.AlreadyExists => Conflict(result.Error.Message),
            _ => BadRequest(result.Error.Message)
        };
    }
}