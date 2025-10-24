namespace SparePartsWarehouse.Domain.Entities;

public class SupplyItem
{
    public Guid Id { get; private init; }
    public string Model { get; private init; }
    public int Amount { get; private init; }
    public string CompatibleEquipmentModels { get; private init; }

    public SupplyItem(string model, int amount, string compatibleEquipmentModels)
    {
        Id = Guid.NewGuid();
        Model = model;
        Amount = amount;
        CompatibleEquipmentModels = compatibleEquipmentModels;
    }
}