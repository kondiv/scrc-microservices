using EquipmentService.Domain.Enums;
using MediatR;
using Shared.ResultPattern;

namespace EquipmentService.Api.Features.Equipments.Create;

internal sealed record CreateEquipmentCommand(string Name, string SerialNumber, Category Category, Voltage Voltage,
    DateTime DeliveredAt, DateTime WarrantyExpiresAt) : IRequest<Result<Guid>>;