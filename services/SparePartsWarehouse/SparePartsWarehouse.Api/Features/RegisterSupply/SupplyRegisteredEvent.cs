using MediatR;

namespace SparePartsWarehouse.Api.Features.RegisterSupply;

internal sealed record SupplyRegisteredEvent(Guid SupplyId) : INotification;