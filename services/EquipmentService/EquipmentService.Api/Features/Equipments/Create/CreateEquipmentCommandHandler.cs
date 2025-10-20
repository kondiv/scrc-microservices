using EquipmentService.Domain.Entities;
using EquipmentService.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace EquipmentService.Api.Features.Equipments.Create;

internal sealed class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, Result<Guid>>
{
    private readonly EquipmentServiceDbContext _context;
    private readonly IValidator<CreateEquipmentCommand> _validator;
    private readonly ILogger<CreateEquipmentCommandHandler> _logger;

    public CreateEquipmentCommandHandler(
        EquipmentServiceDbContext context,
        ILogger<CreateEquipmentCommandHandler> logger,
        IValidator<CreateEquipmentCommand> validator)
    {
        _context = context;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreateEquipmentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{dateTime:yyyy-MM-dd HH:mm:ss}] Creating new equipment", DateTime.UtcNow);
        await _validator.ValidateAndThrowAsync(command, cancellationToken); 
        
        var exist = await _context
            .Equipments
            .FirstOrDefaultAsync(r => r.SerialNumber == command.SerialNumber, cancellationToken) != null;

        if (exist)
        {
            _logger.LogError("[{dateTime:yyyy-MM-dd HH:mm:ss}] Equipment was not created, because it is already exists",
                DateTime.UtcNow);
            return Result<Guid>.Failure(new AlreadyExistsError("Equipment with such S/N already exists"));
        }
        
        var equipment = new Equipment(
            command.Name,
            command.SerialNumber,
            command.Category,
            command.Voltage,
            command.DeliveredAt,
            command.WarrantyExpiresAt);

        try
        {
            await _context.Equipments.AddAsync(equipment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when(e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            _logger.LogError(e, "[{dateTime:yyyy-MM-dd HH:mm:ss}] Creating new equipment failed", DateTime.UtcNow);
            return Result<Guid>.Failure(new AlreadyExistsError("Equipment already exists"));
        }

        return Result<Guid>.Success(equipment.Id);
    }
}