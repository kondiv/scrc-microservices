using MediatR;
using Shared.ResultPattern;
using SparePartsWarehouse.Domain.ValueTypes;

namespace SparePartsWarehouse.Api.Features.Supplies;

internal sealed record AcceptSupplyRequest(Supply Supply) : IRequest<Result>;