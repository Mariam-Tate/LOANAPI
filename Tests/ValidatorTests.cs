using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finall_Project.Validators;
using FluentValidation.TestHelper;
using LoanAPI.Data;
using Finall_Project.Enums;

[TestClass]
public class ValidatorTests
{
    private UserValidator _validator;
    private LoanValidator _loanValidator;

    [TestInitialize]
    public void TestInitialize()
    {
        _validator = new UserValidator();
        _loanValidator = new LoanValidator();

    }
    [TestMethod]
    public void FirstNameIsRequired()
    {
        // Arrange
        var user = new User { FirstName = null };
        // Act

        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
              .WithErrorMessage("First Name is required");
    }

    [TestMethod]
    public void FirstNameIsTooLong()
    {
        // Arrange
        var user = new User { FirstName = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin hendrerit ex ut metus faucibus, sit amet lacinia arcu luctus." };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
              .WithErrorMessage("First Name is too long, enter less than 50 letters");
    }
    [TestMethod]
    public void Should_Return_Error_When_LastName_Is_Null()
    {
        // Arrange
        var user = new User { LastName = null };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [TestMethod]
    public void Should_Return_Error_When_LastName_Is_Too_Long()
    {
        // Arrange
        var user = new User { LastName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [TestMethod]
    public void Should_Return_Error_When_Age_Is_Less_Than_18()
    {
        // Arrange
        var user = new User { Age = 16 };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Age);
    }

    [TestMethod]
    public void Should_Return_Error_When_Email_Is_Null()
    {
        // Arrange
        var user = new User { Email = null };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestMethod]
    public void Should_Return_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var user = new User { Email = "invalidemail" };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestMethod]
    public void Should_Return_Error_When_Salary_Is_Outside_Range()
    {
        // Arrange
        var user = new User { Salary = 15000 };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Salary);
    }

    [TestMethod]
    public void Should_Return_Error_When_UserName_Is_Null()
    {
        // Arrange
        var user = new User { UserName = null };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [TestMethod]
    public void Should_Return_Error_When_UserName_Is_Too_Long()
    {
        // Arrange
        var user = new User { UserName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz" };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [TestMethod]
    public void Should_Return_Error_When_Password_Is_Null()
    {
        // Arrange
        var user = new User { Password = null };

        // Act
        var result = _validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [TestMethod]
    public void LoanValidator_ValidLoan_ReturnsTrue()
    {
        // Arrange
        var loan = new Loan
        {
            LoanType = LoanType.Auto,
            Amount = 50000,
            Currency = Currency.GEL,
            LoanPeriod = 10,
            Status = Status.InProcess
        };

        // Act
        var result = _loanValidator.Validate(loan);

        // Assert
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void LoanValidator_InvalidLoan_ReturnsFalse()
    {
        // Arrange
        var loan = new Loan
        {
            LoanType = LoanType.Auto,
            Amount = 50000,
            Currency = Currency.GEL,
            LoanPeriod = 1000,
            Status = Status.InProcess
        };

        // Act
        var result = _loanValidator.Validate(loan);

        // Assert
        Assert.IsFalse(result.IsValid);
    }
}


