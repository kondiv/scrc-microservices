namespace SparePartsWarehouse.Domain.Entities;

public sealed class CompatibleEquipment
{
    public int Id { get; init; }
    public string Model { get; init; }
    public int MinVoltage { get; init; }
    public int MaxVoltage { get; init; }
    public ICollection<SparePartCompatibleEquipment> SparePartCompatibleEquipments { get; private set; }

    public CompatibleEquipment(int id, string model, int minVoltage, int maxVoltage)
    {
        Id = id;
        Model = model;
        MinVoltage = minVoltage;
        MaxVoltage = maxVoltage;
        SparePartCompatibleEquipments = new List<SparePartCompatibleEquipment>();
    }
}