using Common.Validators;
using Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Tests.AuthenticationService.Validators;

public class FullNameValidatorTests
{
    private readonly FullNameValidator _validator = new FullNameValidator();

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WhenFirstNameIsEmptyOrWhiteSpace_ShouldHaveErrorForFirstName(string firstName)
    {
        // Arrange
        var invalidFullName = new FullName(firstName, "Surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name is required");
    }

    [Fact]
    public void Validate_WhenFirstNameLengthIsTooShort_ShouldHaveErrorForFirstName()
    {
        // Arrange
        var invalidFullName = new FullName("F", "Surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name must be at least 2 characters");
    }

    [Fact]
    public void Validate_WhenFirstNameLengthIsTooLong_ShouldHaveErrorForFirstName()
    {
        // Arrange
        var invalidFullName = new FullName(new string('F', 41), "Surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name must be at most 40 characters");
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("=-0")]
    [InlineData("/\\|_|)")]
    public void Validate_WhenFirstNameContainsNonLetterSymbol_ShouldHaveErrorForFirstName(string invalidFirstName)
    {
        // Arrange
        var invalidFullName = new FullName(invalidFirstName, "Surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("Must contain only letters");
    }
    
    [Fact]
    public void Validate_WhenFirstNameDoesNotStartWithUpperLetter_ShouldHaveErrorForFirstName()
    {
        // Arrange
        var invalidFullName = new FullName("firstname", "Surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
            .WithErrorMessage("First name must start with upper letter");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WhenSurnameIsEmptyOrWhiteSpace_ShouldHaveErrorForSurname(string surname)
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", surname, "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Surname is required");
    }

    [Fact]
    public void Validate_WhenSurnameIsTooShort_ShouldHaveErrorForSurname()
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "s", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Surname must be at least 2 characters");
    }

    [Fact]
    public void Validate_WhenSurnameLengthIsTooLong_ShouldHaveErrorForSurname()
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", new string('S', 61), "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Surname must be at most 60 characters");
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("=-0")]
    [InlineData("/\\|_|)")]
    public void Validate_WhenSurnameContainsNonLetterSymbol_ShouldHaveErrorForSurname(string invalidSurname)
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", invalidSurname, "Patronymic");
        
        // Act
        var result =  _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Must contain only letters");
    }

    [Fact]
    public void Validate_WhenSurnameDoesNotStartWithUpperLetter_ShouldHaveErrorForSurname()
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "surname", "Patronymic");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage("Surname must start with upper letter");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WhenPatronymicIsEmptyOrWhiteSpace_ShouldHaveErrorForPatronymic(string invalidPatronymic)
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "Surname", invalidPatronymic);
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic)
            .WithErrorMessage("Patronymic is required");
    }

    [Fact]
    public void Validate_WhenPatronymicLengthIsTooShort_ShouldHaveErrorForPatronymic()
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "Surname", "P");
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic)
            .WithErrorMessage("Patronymic must be at least 2 characters");
    }

    [Fact]
    public void Validate_WhenPatronymicLengthIsTooLong_ShouldHaveErrorForPatronymic()
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "Surname", new string('P', 61));
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic)
            .WithErrorMessage("Patronymic must be at most 60 characters");
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("=-0")]
    [InlineData("/\\|_|)")]
    public void Validate_WhenPatronymicContainsNonLetterSymbol_ShouldHaveErrorForPatronymic(string invalidPatronymic)
    {
        // Arrange
        var invalidFullName = new FullName("Firstname", "Surname", invalidPatronymic);
        
        // Act
        var result = _validator.TestValidate(invalidFullName);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Patronymic)
            .WithErrorMessage("Must contain only letters");
    }
}