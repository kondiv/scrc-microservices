using FluentValidation;

namespace EquipmentService.Api.Features.Equipments.List;

internal sealed class ListEquipmentRequestValidator : AbstractValidator<ListEquipmentRequest>
{
    public ListEquipmentRequestValidator()
    {
        RuleFor(r => r.Limit)
            .NotNull().WithMessage("Limit is required")
            .GreaterThan(0).WithMessage("Limit must be greater than 0")
            .LessThan(100).WithMessage("Limit must be less than 100");
    }
}