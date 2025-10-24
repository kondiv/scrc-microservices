using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;
using SparePartsWarehouse.Infrastructure;

namespace SparePartsWarehouse.Api.Features.WarehouseItems;

internal sealed class DecreaseAmountRequestHandler : IRequestHandler<DecreaseAmountRequest, Result>
{
    private readonly SparePartsWarehouseContext _context;
    private readonly ILogger<DecreaseAmountRequestHandler> _logger;

    public DecreaseAmountRequestHandler(SparePartsWarehouseContext context, ILogger<DecreaseAmountRequestHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(DecreaseAmountRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Decreasing amount of spare part {id}", request.SparePartId);

        var sparePart = await _context
            .SpareParts
            .FindAsync([request.SparePartId], cancellationToken);

        if (sparePart is null)
        {
            _logger.LogError("Cannot find spare part {id}", request.SparePartId);
            return Result.Failure(new NotFoundError($"Cannot find spare part with id {request.SparePartId}"));
        }

        if (sparePart.Amount < request.Amount)
        {
            _logger.LogWarning("Try to decrease amount below zero, current amount: {amount}, requested", sparePart.Amount);
            return Result.Failure(new Error($"The quantity cannot be reduced to the requested one, " +
                                            $"the maximum available is {sparePart.Amount}", ErrorCode.SomeError));
        }
        
        sparePart.DecreaseAmount(request.Amount);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Amount of spare part has been already reduced by someone");
            return Result.Failure(new DbUpdateConcurrencyError("Cannot reduce amount of spare part in warehouse." +
                                                               " Data is not up to date." +
                                                               " Update your data and try again"));
        }

        return Result.Success();
    }
}