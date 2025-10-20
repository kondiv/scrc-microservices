using EquipmentService.Domain.Enums;

namespace EquipmentService.Api.ApiRequests;

public sealed record CreateEquipmentApiRequest(string Name, string SerialNumber, Category Category, Voltage Voltage,
    DateTime DeliveredAt, DateTime WarrantyExpiresAt);