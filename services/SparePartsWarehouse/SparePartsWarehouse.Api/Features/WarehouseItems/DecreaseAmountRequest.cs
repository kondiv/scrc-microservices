using MediatR;
using Shared.ResultPattern;

namespace SparePartsWarehouse.Api.Features.WarehouseItems;

internal sealed record DecreaseAmountRequest(Guid SparePartId, int Amount) : IRequest<Result>;