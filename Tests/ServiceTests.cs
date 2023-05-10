using Finall_Project.Controllers;
using Finall_Project.Helpers;
using Finall_Project.Services;
using LoanAPI.Data;
using LoanAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*[TestClass]
public class ServiceTests
{

    [TestMethod]
    public void GetUserById_ShouldReturnUserWithSameID()
    {
        // Arrange
        var contextOptions = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: "LoanAPI")
            .Options;
        var context = new UserContext(contextOptions);
       // context.Users.Add(GetDemoUser());

        var appSettings = Options.Create(new AppSettings { Secret = "this is secret text" });
        var userService = new UserService(context);
        var controller = new UserController(appSettings, userService);

        // Act
        var result = controller.GetUserById(1) as OkObjectResult;
        var user = result.Value as User;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, user.Id);
    }
}*/