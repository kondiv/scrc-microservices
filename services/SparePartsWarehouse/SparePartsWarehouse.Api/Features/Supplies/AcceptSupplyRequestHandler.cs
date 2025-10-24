using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using SparePartsWarehouse.Domain.Entities;
using SparePartsWarehouse.Domain.ValueTypes;
using SparePartsWarehouse.Infrastructure;

namespace SparePartsWarehouse.Api.Features.Supplies;

internal sealed class AcceptSupplyRequestHandler : IRequestHandler<AcceptSupplyRequest, Result>
{
    private readonly SparePartsWarehouseContext _context;
    private readonly ILogger<AcceptSupplyRequestHandler> _logger;
    
    public AcceptSupplyRequestHandler(SparePartsWarehouseContext context, ILogger<AcceptSupplyRequestHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> Handle(AcceptSupplyRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Accepting supply from {supplier}. Supply amount {amount}", request.Supply.Supplier,
            request.Supply.Items.Count);

        var items = request.Supply.Items;
        
        var models = items.Select(i => i.Model).Distinct().ToList();

        var warehouseSpareParts = await _context
            .SpareParts
            .Where(s => models.Contains(s.Model))
            .ToDictionaryAsync(s => s.Model, cancellationToken);

        List<SparePart> sparePartsToCreate = [];

        foreach (var item in items)
        {
            if (warehouseSpareParts.TryGetValue(item.Model, out var warehouseSparePart))
            {
                warehouseSparePart.IncreaseAmount(item.Amount);
            }
            else
            {
                sparePartsToCreate.Add(new SparePart(
                        item.Model,
                        item.Amount,
                        item.CompatibleEquipmentModels));
            }
        }

        if (sparePartsToCreate.Count != 0)
        {
            await _context.AddRangeAsync(sparePartsToCreate, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}