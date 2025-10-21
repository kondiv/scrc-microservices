namespace SparePartsWarehouse.Domain.Entities;

public sealed class SparePart
{
    public Guid Id { get; private init; }
    public string Model { get; private init; }
    public int Amount { get; private set; }
    public string CompatibleEquipmentModels { get; private set; }
    public DateTime FirstTimeDeliveredAt { get; init; }
    public DateTime LastUpdatedAt { get; set; }

    public SparePart(string model, int amount, string compatibleEquipmentModels)
    {
        Id = Guid.NewGuid();
        Model = model;
        Amount = amount;
        CompatibleEquipmentModels = compatibleEquipmentModels;
        FirstTimeDeliveredAt = LastUpdatedAt = DateTime.UtcNow;
    }

    public void IncreaseAmount(int amount)
    {
        Amount += amount;
    }

    public void DecreaseAmount(int amount)
    {
        if (amount > Amount)
        {
            throw new InvalidOperationException("Amount cannot be decreased to negative value");
        }

        Amount -= amount;
    }
}