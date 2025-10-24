namespace SparePartsWarehouse.Domain.ValueTypes;

public record struct Supply(string Supplier, DateTime ArrivedAt, List<SupplyItem> Items);