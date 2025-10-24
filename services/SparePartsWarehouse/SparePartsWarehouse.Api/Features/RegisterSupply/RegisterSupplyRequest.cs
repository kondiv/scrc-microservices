using MediatR;
using Shared.ResultPattern;
using SparePartsWarehouse.Domain.Entities;

namespace SparePartsWarehouse.Api.Features.RegisterSupply;

internal sealed record RegisterSupplyRequest(List<SupplyItem> SupplyItems, DateTime ArrivedAt) : IRequest<Result>;