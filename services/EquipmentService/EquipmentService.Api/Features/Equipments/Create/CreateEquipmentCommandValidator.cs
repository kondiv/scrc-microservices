using FluentValidation;
using FluentValidation.Validators;

namespace EquipmentService.Api.Features.Equipments.Create;

internal sealed class CreateEquipmentCommandValidator : AbstractValidator<CreateEquipmentCommand>
{
    public CreateEquipmentCommandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(128).WithMessage("Name must be at most 128 characters long");
        
        RuleFor(r => r.SerialNumber)
            .NotEmpty().WithMessage("Serial number is required")
            .MinimumLength(2).WithMessage("Serial number must be at least 2 characters long")
            .MaximumLength(128).WithMessage("Serial number must be at most 128 characters long");

        RuleFor(r => r.Category)
            .NotNull().WithMessage("Category is required");
        
        RuleFor(r => r.Voltage)
            .NotNull().WithMessage("Voltage is required");
        
        RuleFor(r => r.DeliveredAt)
            .NotNull().WithMessage("Delivery date is required")
            .Must(BeValidDate).WithMessage("Delivered at cannot be date in future");
        
        RuleFor(r => r.WarrantyExpiresAt)
            .NotNull().WithMessage("Warranty expiration date is required")
            .Must(BeValidDate).WithMessage("Warranty expiration date cannot be date in future");
    }

    private static bool BeValidDate(DateTime date)
    {
        return date <= DateTime.UtcNow;
    }
}