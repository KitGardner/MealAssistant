using MealAssistant.Controllers;
using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MealAssistant.Tests.Controllers;

public class AccountControllerTests
{
    private AccountController controller;
    private Mock<IAccountService> service;

    [SetUp]
    public void Setup()
    {
        service = new Mock<IAccountService>();
        controller = CreateController(service);
    }

    [Test]
    public async Task GetAccounts_NoUsername_ReturnsAllAccounts()
    {
        // Arrange
        var testAccounts = CreateTestAccounts(3);
        service.Setup(s => s.GetAccounts()).ReturnsAsync(testAccounts);

        // Act
        var result = await controller.GetAccounts(null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountResponse>>());

        var accounts = response.Value as List<AccountResponse>;
        Assert.That(accounts.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetAccounts_UserNameProvided_ReturnsMatchingAccount()
    {
        // Arrange
        var testAccount = CreateTestAccounts(1).First();
        service.Setup(s => s.GetAccount(It.IsAny<string>())).ReturnsAsync(testAccount);

        // Act
        var result = await controller.GetAccounts(testAccount.Username);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountResponse>>());

        var accounts = response.Value as List<AccountResponse>;
        Assert.That(accounts.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetAccounts_NoMatchingUserName_ReturnsNotFound()
    {
        // Arrange
        service.Setup(s => s.GetAccount(It.IsAny<string>())).ReturnsAsync((Account?)null);

        // Act
        var result = await controller.GetAccounts("username");

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAccount_ReturnsAccount()
    {
        // Arrange
        var testAccount = CreateTestAccounts(1).First();
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(testAccount);

        // Act
        var result = await controller.GetAccount(testAccount.Id);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<AccountResponse>());
    }

    [Test]
    public async Task GetAccount_WhenMissing_ReturnsNotFound()
    {
        // Arrange
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        // Act
        var result = await controller.GetAccount(Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task CreateAccount_ReturnsCreatedAtAction()
    {
        // Arrange
        service.Setup(s => s.CreateAccount(It.IsAny<Account>()))
            .Callback<Account>(a => a.Id = Guid.NewGuid())
            .Returns(Task.CompletedTask);

        var newAccount = new AccountRequest
        {
            FirstName = "Test",
            LastName = "Auto",
            Email = "Test@auto.com",
            Username = "Test1",
            Password = "TestPassword"
        };

        // Act
        var result = await controller.CreateAccount(newAccount);

        // Assert
        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());

        var response = result.Result as CreatedAtActionResult;
        Assert.That(response.Value, Is.TypeOf<AccountResponse>());

        var createdAccount = response.Value as AccountResponse;
        Assert.That(createdAccount.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(createdAccount.FirstName, Is.EqualTo(newAccount.FirstName));
        Assert.That(createdAccount.LastName, Is.EqualTo(newAccount.LastName));
        Assert.That(createdAccount.Email, Is.EqualTo(newAccount.Email));
        Assert.That(createdAccount.Username, Is.EqualTo(newAccount.Username));
    }

    [Test]
    public async Task CreateAccount_InvalidAccountRequest_ReturnsBadRequest()
    {
        // Act
        var result = await controller.CreateAccount(null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task UpdateAccount_AccountObjectNull_ReturnsBadRequest()
    {
        // Act
        var result = await controller.UpdateAccount(Guid.NewGuid(), null);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task UpdateAccount_WhenBodyIdMismatch_ReturnsBadRequest()
    {
        // Act
        var result = await controller.UpdateAccount(Guid.NewGuid(), new AccountRequest { Id = Guid.NewGuid() });

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateAccount_NoMatchingAccount_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        // Act
        var result = await controller.UpdateAccount(id, new AccountRequest { Id = id });

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task UpdateAccount_UpdatesSuccessfully_ReturnsNoContent()
    {
        // Arrange
        var testAccount = CreateTestAccounts(1).First();
        testAccount.Id = Guid.NewGuid();
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((testAccount));
        service.Setup(s => s.UpdateAccount(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateAccount(testAccount.Id, new AccountRequest { Id = testAccount.Id });

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteAccount_NoMatchingAccount_ReturnsNotFound()
    {
        // Arrange
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account?)null);

        // Act
        var result = controller.DeleteAccount(Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteAccount_DeletesSuccessfully_ReturnsNoContent()
    {
        // Arrange
        var testAccount = CreateTestAccounts(1).First();
        testAccount.Id = Guid.NewGuid();
        service.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(testAccount);
        service.Setup(s => s.DeleteAccount(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        var result = controller.DeleteAccount(testAccount.Id);

        // Assert
        Assert.That(result.Result, Is.TypeOf<NoContentResult>());
    }

    private AccountController CreateController(Mock<IAccountService> service)
    {
        return new AccountController(new Mock<ILogger<AccountController>>().Object, service.Object);
    }

    private List<Account> CreateTestAccounts(int quantity)
    {
        var accounts = new List<Account>();

        for (int i = 0; i < quantity; i++)
        {
            accounts.Add(new Account
            {
                Id = Guid.NewGuid(),
                FirstName = $"Test {i}",
                LastName = "Account",
                Email = $"test{i}@test.com",
                Username = $"TestUser{i}"
            });
        }

        return accounts;
    }
}
