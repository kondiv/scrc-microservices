namespace SparePartsWarehouse.Domain.Entities;

public sealed class SparePart
{
    public Guid Id { get; private init; }
    public string Model { get; private init; }
    public ICollection<SparePartCompatibleEquipment> SparePartCompatibleEquipments { get; private set; }

    public SparePart(string model, ICollection<SparePartCompatibleEquipment>? compatibleEquipments)
    {
        Id = Guid.NewGuid();
        Model = model;
        SparePartCompatibleEquipments = compatibleEquipments ?? new List<SparePartCompatibleEquipment>();
    }

    public void AddCompatibleEquipment(CompatibleEquipment compatibleEquipment)
    {
        SparePartCompatibleEquipments.Add(new SparePartCompatibleEquipment(this, compatibleEquipment));
    }

    public void RemoveCompatibleEquipment(CompatibleEquipment compatibleEquipment)
    {
        SparePartCompatibleEquipments.Remove(new SparePartCompatibleEquipment(this, compatibleEquipment));
    }
}