using EquipmentService.Api.Dtos;
using EquipmentService.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace EquipmentService.Api.Features.Equipments.Get;

internal sealed class GetEquipmentRequestHandler : IRequestHandler<GetEquipmentRequest, Result<EquipmentDto>>
{
    private readonly EquipmentServiceDbContext _dbContext;
    private readonly ILogger<GetEquipmentRequestHandler> _logger;

    public GetEquipmentRequestHandler(
        EquipmentServiceDbContext dbContext,
        ILogger<GetEquipmentRequestHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<EquipmentDto>> Handle(GetEquipmentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{dateTime:u}] Requesting for equipment with id: {id}]", DateTime.UtcNow, request.Id);
        
        var equipment = await _dbContext
            .Equipments
            .Select(eq => new EquipmentDto(eq.Id, eq.Name, eq.SerialNumber, eq.Status, eq.Category, eq.DeliveredAt))
            .AsNoTracking()
            .FirstOrDefaultAsync(eq => eq.Id.Equals(request.Id), cancellationToken);

        if (equipment is null)
        {
            _logger.LogError("[{dateTime:u}] Cannot find equipment with id: {id}", DateTime.UtcNow, request.Id);
            return Result<EquipmentDto>.Failure(new NotFoundError($"Cannot find equipment with id: {request.Id}"));
        }

        return Result<EquipmentDto>.Success(equipment);
    }
}