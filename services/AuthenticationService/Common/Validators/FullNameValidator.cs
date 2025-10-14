using Domain.ValueObjects;
using FluentValidation;

namespace Common.Validators;

public sealed class FullNameValidator : AbstractValidator<FullName>
{
    public FullNameValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters")
            .MaximumLength(40).WithMessage("First name must be at most 40 characters")
            .Must(StartWithUpperLetter).WithMessage("First name must start with upper letter");
        
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required")
            .MinimumLength(2).WithMessage("Surname must be at least 2 characters")
            .MaximumLength(60).WithMessage("Surname must be at most 60 characters")
            .Must(StartWithUpperLetter).WithMessage("Surname must start with upper letter");
        
        RuleFor(x => x.Patronymic)
            .NotEmpty().WithMessage("Patronymic is required")
            .MinimumLength(2).WithMessage("Patronymic must be at least 2 characters")
            .MaximumLength(60).WithMessage("Patronymic must be at most 60 characters")
            .Must(StartWithUpperLetter).WithMessage("Patronymic must start with upper letter");
    }

    private static bool StartWithUpperLetter(string name)
    {
        return char.IsUpper(name[0]);
    }
}