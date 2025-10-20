using EquipmentService.Domain.Enums;

namespace EquipmentService.Domain.Entities;

public class Equipment
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public string SerialNumber { get; private init; }
    public Category Category { get; private init; }
    public Status Status { get; private set; }
    public Voltage Voltage { get; private init; }
    public DateTime DeliveredAt { get; private init; }
    public DateTime? InUseSince { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }
    public DateTime WarrantyExpiresAt { get; private init; }

    public Equipment(
        string name,
        string serialNumber,
        Category category,
        Voltage voltage,
        DateTime deliveredAt,
        DateTime warrantyExpiresAt)
    {
        Id = Guid.NewGuid();
        Name = name;
        SerialNumber = serialNumber;
        Category = category;
        Status = Status.Arrived;
        Voltage = voltage;
        DeliveredAt = deliveredAt;
        WarrantyExpiresAt = warrantyExpiresAt;
    }

    public void InUse()
    {
        Status = Status.InUse;
        InUseSince = DateTime.UtcNow;
    }

    public void ChangeStatus(Status status)
    {
        Status = status;
    }

    public void Updated()
    {
        LastUpdatedAt = DateTime.UtcNow;
    }
}