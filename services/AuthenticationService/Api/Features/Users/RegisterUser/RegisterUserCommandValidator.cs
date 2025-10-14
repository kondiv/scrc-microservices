using System.Text.RegularExpressions;
using Common.Validators;
using FluentValidation;

namespace Api.Features.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const string SpecialCharacters = "!@#$%^&*-_";
    
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .SetValidator(new FullNameValidator());

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .MinimumLength(5).WithMessage("Email must be at least 5 characters")
            .MaximumLength(128).WithMessage("Email must be at most 128 characters")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login is required")
            .MinimumLength(4).WithMessage("Login must be at least 4 characters")
            .MaximumLength(128).WithMessage("Login must be at most 128 characters")
            .Must(ContainOnlyAvailableSymbols).WithMessage("Login must contain only latin symbols and digits");

        RuleFor(x => x.PlainPassword)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Must(ContainAtLeastOneDigit).WithMessage("Password must contain at least one digit")
            .Must(ContainAtLeastOneUpperLetter).WithMessage("Password must contain at least one upper letter")
            .Must(ContainAtLeastOneSpecialCharacter).WithMessage(
                "Password must contain at least one special character " +
                $"\"{SpecialCharacters}\"");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .MinimumLength(4).WithMessage("Role must be at least 4 characters")
            .Must(ConsistOfUpperLetters).WithMessage("Role must contain only upper letters")
            .Must(BeWithoutWhitespace).WithMessage("Role must not contain whitespaces");
    }

    private static bool ContainOnlyAvailableSymbols(string login)
    {
        const string pattern = @"^[a-zA-Z0-9]+$";
        return Regex.IsMatch(login, pattern);
    }

    private static bool ContainAtLeastOneDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private static bool ContainAtLeastOneUpperLetter(string password)
    {
        return password.Any(char.IsUpper);
    }

    private static bool ContainAtLeastOneSpecialCharacter(string password)
    {
        return password.Any(ch => SpecialCharacters.Contains(ch));
    }

    private static bool ConsistOfUpperLetters(string role)
    {
        return role.All(char.IsUpper);
    }

    private static bool BeWithoutWhitespace(string role)
    {
        return !role.Any(char.IsWhiteSpace);
    }
}