using EquipmentService.Api.Dtos;
using MediatR;
using Shared.ResultPattern;

namespace EquipmentService.Api.Features.Equipments.Get;

internal sealed record GetEquipmentRequest(Guid Id) : IRequest<Result<EquipmentDto>>;