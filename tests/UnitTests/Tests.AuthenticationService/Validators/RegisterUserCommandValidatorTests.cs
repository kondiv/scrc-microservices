using Api.Features.Users.RegisterUser;
using Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Tests.AuthenticationService.Validators;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new RegisterUserCommandValidator();
    private readonly FullName _fullName = new FullName("Firstname", "Surname", "Patronymic");

    public static IEnumerable<object[]> GetLongLogins()
    {
        yield return [new string('l', 129)];
        yield return [new string('l', 200)];
        yield return [new string('l', 300)];
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WhenEmailIsEmptyOrWhiteSpace_ShouldHaveErrorForEmail(string invalidEmail)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, invalidEmail, "login", "Password123!",
            "ROLE");

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Theory]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user@.")]
    [InlineData("user@.com")]
    [InlineData("@")]
    [InlineData("@.")]
    [InlineData("user@domain..com")]
    [InlineData(".user@domain.com")]
    [InlineData("user.@domain.com")]
    [InlineData("user..name@domain.com")]
    [InlineData("user@-domain.com")]
    [InlineData("user@domain-.com")]
    [InlineData("user@domain.c")]
    [InlineData("user@domain.1")]
    [InlineData("user@domain.a")]
    [InlineData("user@domain.123")]
    [InlineData("user name@domain.com")]
    [InlineData("user<tag>@domain.com")]
    [InlineData("user[tagname]@domain.com")]
    [InlineData("user(tag)@domain.com")]
    [InlineData("user,tag@domain.com")]
    [InlineData("user;tag@domain.com")]
    [InlineData("user:tag@domain.com")]
    public void Validate_WhenEmailIsNotEmail_ShouldHaveErrorForEmail(string invalidEmail)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, invalidEmail, "login",
            "Password123!", "ROLE");

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address");
    }

    [Fact]
    public void Validate_WhenEmailIsTooShort_ShouldHaveErrorForEmail()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "e@e", "login",
            "Password123!", "ROLE");

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be at least 5 characters");
    }

    [Fact]
    public void Validate_WhenEmailIsTooLong_ShouldHaveErrorForEmail()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, new string('e', 128) + "@email.com",
            "login", "Password123!", "ROLE");

        // Act
        var result = _validator.TestValidate(invalidCommand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email must be at most 128 characters");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenLoginIsEmptyOrWhiteSpace_ShouldHaveErrorForLogin(string invalidLogin)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", invalidLogin,
            "Password123!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage("Login is required");
    }

    [Theory]
    [InlineData("l")]
    [InlineData("lo")]
    [InlineData("log")]
    public void Validate_WhenLoginIsTooShort_ShouldHaveErrorForLogin(string invalidLogin)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", invalidLogin,
            "Password123!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage("Login must be at least 4 characters");
    }

    [Theory]
    [MemberData(nameof(GetLongLogins))]
    public void Validate_WhenLoginIsTooLong_ShouldHaveErrorForLogin(string invalidLogin)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", invalidLogin,
            "Password123!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage("Login must be at most 128 characters");
    }

    [Theory]
    [InlineData("логин")]
    [InlineData("login|\\")]
    [InlineData("lo/6\\in")]
    [InlineData("логин!@#$")]
    [InlineData("!@#$")]
    public void Validate_WhenLoginContainNotOnlyLatinLettersAndDigits_ShouldHaveErrorForLogin(string invalidLogin)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", invalidLogin,
            "Password123!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Login)
            .WithErrorMessage("Login must contain only latin symbols and digits");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenPasswordIsMissing_ShouldHaveErrorForPassword(string invalidPassword)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            invalidPassword, "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlainPassword)
            .WithErrorMessage("Password is required");
    }

    [Fact]
    public void Validate_WhenPasswordIsTooShort_ShouldHaveErrorForPlainPassword()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Pass1!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlainPassword)
            .WithErrorMessage("Password must be at least 8 characters");
    }

    [Fact]
    public void Validate_WhenPasswordIsMissingDigit_ShouldHaveErrorForPlainPassword()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Password!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlainPassword)
            .WithErrorMessage("Password must contain at least one digit");
    }

    [Fact]
    public void Validate_WhenPasswordIsMissingUpperLetter_ShouldHaveErrorForPlainPassword()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "password123!", "ROLE");

        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlainPassword)
            .WithErrorMessage("Password must contain at least one upper letter");
    }

    [Theory]
    [InlineData("Password123")]
    [InlineData("Password123{}()[]\"'+")]
    public void Validate_WhenPasswordIsMissingSpecialSymbolOrSpecialSymbolNotAllowed_ShouldHaveErrorForPlainPassword(string invalidPassword)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            invalidPassword, "ROLE");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlainPassword)
            .WithErrorMessage("Password must contain at least one special character \"!@#$%^&*-_\"");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenRoleIsMissing_ShouldHaveErrorForRole(string invalidRole)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Password123!", invalidRole);
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role is required");
    }

    [Fact]
    public void Validate_WhenRoleIsTooShort_ShouldHaveErrorForRole()
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Password123!", "RO");
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role must be at least 4 characters");
    }

    [Theory]
    [InlineData("Role")]
    [InlineData("rOle")]
    [InlineData("roLe")]
    [InlineData("rolE")]
    public void Validate_WhenRoleConsistsNotOnlyOfUpperLetters_ShouldHaveErrorForRole(string invalidRole)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Password123!", invalidRole);
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role must contain only upper letters");
    }

    [Theory]
    [InlineData(" ROLE")]
    [InlineData("ROLE ")]
    [InlineData("RO LE")]
    [InlineData(" RO LE ")]
    public void Validate_WhenRoleHasWhiteSpaces_ShouldHaveErrorForRole(string invalidRole)
    {
        // Arrange
        var invalidCommand = new RegisterUserCommand(_fullName, "email@email.com", "login",
            "Password123!", invalidRole);
        
        // Act
        var result = _validator.TestValidate(invalidCommand);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Role)
            .WithErrorMessage("Role must not contain whitespaces");
    }

    [Fact]
    public void Validate_WhenDataIsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var validCommand = new RegisterUserCommand(_fullName, "email@email.com", "login123",
            "Password123!", "ROLE");
        
        // Act
        var result = _validator.TestValidate(validCommand);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}