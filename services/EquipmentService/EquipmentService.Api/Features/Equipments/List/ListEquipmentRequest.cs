using EquipmentService.Api.Common;
using EquipmentService.Api.Dtos;
using MediatR;
using Shared.ResultPattern;

namespace EquipmentService.Api.Features.Equipments.List;

internal sealed record ListEquipmentRequest(int Limit, string? CursorToken) : IRequest<Result<PaginatedListResponse<EquipmentDto>>>;