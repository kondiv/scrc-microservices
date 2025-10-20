using EquipmentService.Domain.Enums;

namespace EquipmentService.Api.Dtos;

public record EquipmentDto(Guid Id, string Name, string SerialNumber, Status Status, Category Category, DateTime DeliveredAt);