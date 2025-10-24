namespace SparePartsWarehouse.Domain.ValueTypes;

public record struct SupplyItem(string Model, int Amount, string CompatibleEquipmentModels);