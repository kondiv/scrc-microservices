namespace SparePartsWarehouse.Domain.Entities;

public class Supply
{
    public Guid Id { get; private init; }
    public ICollection<SupplyItem> SupplyItems { get; private set; } = [];
    public DateTime ArrivedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private Supply()
    {
        
    }

    public Supply(ICollection<SupplyItem> supplyItems, DateTime arrivedAt)
    {
        Id = Guid.NewGuid();
        SupplyItems = supplyItems;
        ArrivedAt = arrivedAt;
    }

    public void AddSupplyItems(ICollection<SupplyItem> supplyItems)
    {
        SupplyItems = supplyItems;
    }
}