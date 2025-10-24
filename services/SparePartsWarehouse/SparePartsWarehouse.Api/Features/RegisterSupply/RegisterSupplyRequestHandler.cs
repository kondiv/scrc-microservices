using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;
using SparePartsWarehouse.Domain.Entities;
using SparePartsWarehouse.Infrastructure;

namespace SparePartsWarehouse.Api.Features.RegisterSupply;

internal sealed class RegisterSupplyRequestHandler : IRequestHandler<RegisterSupplyRequest, Result>
{
    private readonly SparePartsWarehouseContext _context;
    private readonly ILogger<RegisterSupplyRequestHandler> _logger;
    private readonly IMediator _mediator;

    public RegisterSupplyRequestHandler(
        SparePartsWarehouseContext context,
        ILogger<RegisterSupplyRequestHandler> logger,
        IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Result> Handle(RegisterSupplyRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering supply. Arrived at {arrivedAt}", request.ArrivedAt);
        
        var supply = new Supply(request.ArrivedAt);
        supply.AddSupplyItems(request.SupplyItems);

        await _context.Supplies.AddAsync(supply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        try
        {
            await _mediator.Publish(new SupplyRegisteredEvent(supply.Id), cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Result.Success();
    }
}