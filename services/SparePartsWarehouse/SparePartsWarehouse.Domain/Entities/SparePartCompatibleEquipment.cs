namespace SparePartsWarehouse.Domain.Entities;

public sealed class SparePartCompatibleEquipment
{
    public Guid SparePartId { get; init; }
    public int CompatibleEquipmentId { get; init; }

    public SparePart SparePart { get; init; } = null!;
    public CompatibleEquipment CompatibleEquipment { get; init; } = null!;

    public SparePartCompatibleEquipment(Guid sparePartId, int compatibleEquipmentId)
    {
        SparePartId = sparePartId;
        CompatibleEquipmentId = compatibleEquipmentId;
    }

    public SparePartCompatibleEquipment(SparePart sparePart, CompatibleEquipment compatibleEquipment)
    {
        SparePart = sparePart;
        CompatibleEquipment = compatibleEquipment;
    }
}