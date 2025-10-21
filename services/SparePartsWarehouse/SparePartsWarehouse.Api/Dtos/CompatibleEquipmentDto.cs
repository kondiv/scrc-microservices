namespace SparePartsWarehouse.Api.Dtos;

internal sealed record CompatibleEquipmentDto(string Model, int MinVoltage, int MaxVoltage);