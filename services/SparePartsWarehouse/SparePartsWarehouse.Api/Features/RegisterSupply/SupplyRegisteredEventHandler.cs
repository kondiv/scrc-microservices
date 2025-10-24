using MediatR;
using Microsoft.EntityFrameworkCore;
using SparePartsWarehouse.Domain.Entities;
using SparePartsWarehouse.Infrastructure;

namespace SparePartsWarehouse.Api.Features.RegisterSupply;

internal sealed class SupplyRegisteredEventHandler : INotificationHandler<SupplyRegisteredEvent>
{
    private readonly SparePartsWarehouseContext _context;
    private readonly ILogger<SupplyRegisteredEventHandler> _logger;

    public SupplyRegisteredEventHandler(ILogger<SupplyRegisteredEventHandler> logger, SparePartsWarehouseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Handle(SupplyRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transfer spare parts from supply to warehouse");

        var supply = await _context
            .Supplies
            .Include(s => s.SupplyItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == notification.SupplyId, cancellationToken);

        if (supply is null)
        {
            _logger.LogError("Supply was not found. Possible wrong argument provided");
            throw new ArgumentException("Supply was not found", nameof(notification.SupplyId));
        }
        
        var models = supply
            .SupplyItems
            .Select(s => s.Model)
            .Distinct()
            .ToList();

        var warehouseSpareParts = await _context
            .SpareParts
            .Where(s => models.Contains(s.Model))
            .ToDictionaryAsync(s => s.Model, cancellationToken);

        foreach (var item in supply.SupplyItems)
        {
            if (warehouseSpareParts.TryGetValue(item.Model, out var warehouseSparePart))
            {
                warehouseSparePart.IncreaseAmount(item.Amount);
            }
            else
            {
                await _context
                    .AddAsync(new SparePart(
                        item.Model,
                        item.Amount,
                        item.CompatibleEquipmentModels
                    ), cancellationToken);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}