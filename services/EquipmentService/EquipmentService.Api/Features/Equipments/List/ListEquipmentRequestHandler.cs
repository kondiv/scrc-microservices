using EquipmentService.Api.Common;
using EquipmentService.Api.Dtos;
using EquipmentService.Api.Features.Equipments.Get;
using EquipmentService.Domain.ValueTypes;
using EquipmentService.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace EquipmentService.Api.Features.Equipments.List;

internal sealed class ListEquipmentRequestHandler : IRequestHandler<ListEquipmentRequest, Result<PaginatedListResponse<EquipmentDto>>>
{
    private readonly EquipmentServiceDbContext _context;
    private readonly IValidator<ListEquipmentRequest> _validator;
    private readonly ILogger<ListEquipmentRequestHandler> _logger;

    public ListEquipmentRequestHandler(EquipmentServiceDbContext context, ILogger<ListEquipmentRequestHandler> logger, IValidator<ListEquipmentRequest> validator)
    {
        _context = context;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<PaginatedListResponse<EquipmentDto>>> Handle(ListEquipmentRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var query = _context.Equipments.AsQueryable();

        if (!string.IsNullOrEmpty(request.CursorToken))
        {
            var token = Cursor.Decode(request.CursorToken);

            if (token is null)
            {
                return Result<PaginatedListResponse<EquipmentDto>>.Failure(new Error("Invalid cursor token",
                    ErrorCode.SomeError));
            }

            query = query.Where(eq => EF.Functions.LessThanOrEqual(
                ValueTuple.Create(eq.DeliveredAt, eq.Id),
                ValueTuple.Create(token.Date, token.LastId)));
        }

        var items = await query
            .OrderByDescending(eq => eq.DeliveredAt)
            .ThenByDescending(eq => eq.Id)
            .Take(request.Limit + 1)
            .Select(eq => new EquipmentDto(eq.Id, eq.Name, eq.SerialNumber, eq.Status, eq.Category, eq.DeliveredAt))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
        {
            return Result<PaginatedListResponse<EquipmentDto>>.Success(
                new PaginatedListResponse<EquipmentDto>(items, null, false));
        }

        var hasMore = items.Count > request.Limit;

        DateTime? nextDate = hasMore ? items[^1].DeliveredAt : null;
        Guid? nextId = hasMore ? items[^1].Id : null;

        var cursorToken = nextDate is not null && nextId is not null
            ? Cursor.Encode(nextDate.Value, nextId.Value)
            : null;

        if (hasMore)
        {
            items.RemoveAt(items.Count - 1);
        }

        return Result<PaginatedListResponse<EquipmentDto>>.Success(new PaginatedListResponse<EquipmentDto>(items,
            cursorToken, hasMore));
    }
}