using MealAssistant.Controllers;
using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MealAssistant.Tests.Controllers;

public class AccountIngredientControllerTests
{
    private AccountIngredientController controller;
    private Mock<IAccountService> accountServiceMock;
    private Mock<IIngredientService> ingredientServiceMock;
    private Mock<IAccountIngredientService> accountIngredientServiceMock;

    [SetUp]
    public void Setup()
    {
        accountServiceMock = new Mock<IAccountService>();
        ingredientServiceMock = new Mock<IIngredientService>();
        accountIngredientServiceMock = new Mock<IAccountIngredientService>();
        controller = CreateController(accountIngredientServiceMock, accountServiceMock, ingredientServiceMock);
    }

    [Test]
    public async Task GetAccountIngredients_AccountAndIngredientProvided_ReturnsAccountIngredient()
    {
        // Arrange
        var testAccountIngredient = CreateTestAccountIngredients(1, Guid.Empty, Guid.Empty).First();
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(testAccountIngredient);

        // Act
        var result = await controller.GetAccountIngredients(testAccountIngredient.AccountId, testAccountIngredient.IngredientId);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountIngredientResponse>>());

        var returnedIngredients = response.Value as List<AccountIngredientResponse>;
        Assert.That(returnedIngredients.Count, Is.EqualTo(1));
        
    }

    [Test]
    public async Task GetAccountIngredients_AccountProvided_ReturnsAccountIngredients()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var testAccountIngredients = CreateTestAccountIngredients(3, accountId, Guid.Empty);
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientsByAccountId(It.IsAny<Guid>())).ReturnsAsync(testAccountIngredients);

        // Act
        var result = await controller.GetAccountIngredients(accountId, null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountIngredientResponse>>());

        var returnedIngredients = response.Value as List<AccountIngredientResponse>;
        Assert.That(returnedIngredients.Count, Is.EqualTo(3));
        Assert.That(returnedIngredients.All(i => i.AccountId == accountId), Is.True);
    }

    [Test]
    public async Task GetAccountIngredients_IngredientProvided_ReturnsAllAccountsWithIngredient()
    {
        // Arrange
        var ingredientId = Guid.NewGuid();
        var testAccountIngredients = CreateTestAccountIngredients(3, Guid.Empty, ingredientId);
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientsByIngredientId(It.IsAny<Guid>())).ReturnsAsync(testAccountIngredients);

        // Act
        var result = await controller.GetAccountIngredients(null, ingredientId);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountIngredientResponse>>());

        var returnedIngredients = response.Value as List<AccountIngredientResponse>;
        Assert.That(returnedIngredients.Count, Is.EqualTo(3));
        Assert.That(returnedIngredients.All(i => i.IngredientId == ingredientId), Is.True);
    }

    [Test]
    public async Task GetAccountIngredients_NoIdProvided_ReturnsAllAccountIngredients()
    {
        // Arrange
        var testAccountIngredients = CreateTestAccountIngredients(3, Guid.Empty, Guid.Empty);
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredients()).ReturnsAsync(testAccountIngredients);

        // Act
        var result = await controller.GetAccountIngredients(null, null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<AccountIngredientResponse>>());

        var returnedIngredients = response.Value as List<AccountIngredientResponse>;
        Assert.That(returnedIngredients.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetAccountIngredient_WhenMissing_ReturnsNotFound()
    {
        // Arrange
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((AccountIngredient?)null);

        // Act
        var result = await controller.GetAccountIngredient(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAccountIngredient_MatchFound_ReturnsAccountIngredient()
    {
        // Arrange
        var testAccountIngredient = CreateTestAccountIngredients(1, Guid.Empty, Guid.Empty).First();

        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(testAccountIngredient);

        // Act
        var result = await controller.GetAccountIngredient(testAccountIngredient.AccountId, testAccountIngredient.IngredientId);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<AccountIngredientResponse>());
    }

    [Test]
    public async Task CreateAccountIngredient_NullRequest_ReturnsBadRequest()
    {
        // Act
        var result = await controller.CreateAccountIngredient(null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_EmptyAccountId_ReturnsBadRequest()
    {
        // Arrange
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.Empty,
            IngredientId = Guid.NewGuid(),
            Amount = 1
        };

        // Act
        var result = await controller.CreateAccountIngredient(request);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_EmptyIngredientId_ReturnsBadRequest()
    {
        // Arrange
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.Empty,
            Amount = 1
        };

        // Act
        var result = await controller.CreateAccountIngredient(request);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_WhenAccountMissing_ReturnsBadRequest()
    {
        // Arrange
        accountServiceMock.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account?)null);
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 1
        };

        // Act
        var result = await controller.CreateAccountIngredient(request);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_WhenIngredientMissing_ReturnsBadRequest()
    {
        // Arrange
        accountServiceMock.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(new Account());
        ingredientServiceMock.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 1
        };

        // Act
        var result = await controller.CreateAccountIngredient(request);

        // Assert
        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_CreateSuccessful_ReturnsCreated()
    {
        // Arrange
        accountServiceMock.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(new Account());
        ingredientServiceMock.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(new Ingredient());
        accountIngredientServiceMock.Setup(s => s.CreateAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask);
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 1
        };

        // Act
        var result = await controller.CreateAccountIngredient(request);

        // Assert
        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());

        var response = result.Result as CreatedAtActionResult;
        Assert.That(response.Value, Is.TypeOf<AccountIngredientResponse>());

        var createdAccountIngredient = response.Value as AccountIngredientResponse;
        Assert.That(createdAccountIngredient.AccountId, Is.EqualTo(request.AccountId));
        Assert.That(createdAccountIngredient.IngredientId, Is.EqualTo(request.IngredientId));
        Assert.That(createdAccountIngredient.Amount, Is.EqualTo(request.Amount));
    }

    [Test]
    public async Task UpdateAccountIngredient_NullPayload_ReturnsNotFound()
    {
        // Arrange
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((AccountIngredient?)null);

        // Act
        var result = await controller.UpdateAccountIngredient(Guid.NewGuid(), Guid.NewGuid(), null);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task UpdateAccountIngredient_NonMatchingAccountId_ReturnsNotFound()
    {
        // Arrange
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 5
        };

        // Act
        var result = await controller.UpdateAccountIngredient(Guid.NewGuid(), request.IngredientId, request);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateAccountIngredient_NonMatchingIngredientId_ReturnsNotFound()
    {
        // Arrange
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 5
        };

        // Act
        var result = await controller.UpdateAccountIngredient(request.AccountId, Guid.NewGuid(), request);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateAccountIngredient_NoMatchingAccountIngredient_ReturnsNotFound()
    {
        // Arrange
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((AccountIngredient?)null);
        var request = new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 5
        };

        // Act
        var result = await controller.UpdateAccountIngredient(request.AccountId, request.IngredientId, request);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task UpdateAccountIngredient_UpdatesAccountIngredient_ReturnsNoContent()
    {
        // Arrange
        var testAccountIngredient = CreateTestAccountIngredients(1, Guid.Empty, Guid.Empty).First();
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(testAccountIngredient);
        accountIngredientServiceMock.Setup(s => s.UpdateAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask);
        var request = new AccountIngredientRequest
        {
            AccountId = testAccountIngredient.AccountId,
            IngredientId = testAccountIngredient.IngredientId,
            Amount = 5
        };

        // Act
        var result = await controller.UpdateAccountIngredient(testAccountIngredient.AccountId, testAccountIngredient.IngredientId, request);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteAccountIngredient_WhenNotFound_ReturnsNotFound()
    {
        // Arrange
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((AccountIngredient?)null);

        // Act
        var result = await controller.DeleteAccountIngredient(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteAccountIngredient_WhenFound_ReturnsNoContent()
    {
        // Arrange
        accountIngredientServiceMock.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new AccountIngredient());
        accountIngredientServiceMock.Setup(s => s.DeleteAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteAccountIngredient(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    private List<AccountIngredient> CreateTestAccountIngredients(int count, Guid accountId, Guid ingredientId)
    {
        var accountIngredients = new List<AccountIngredient>();
        
        for (int i = 0; i < count; i++)
        {
            accountIngredients.Add(
                new AccountIngredient
                {
                    AccountId = accountId == Guid.Empty ? Guid.NewGuid() : accountId,
                    IngredientId = ingredientId == Guid.Empty ? Guid.NewGuid() : ingredientId,
                    Amount = 1,
                }
            );
        }

        return accountIngredients;
    }

    private AccountIngredientController CreateController(
    Mock<IAccountIngredientService> accountIngredientService,
    Mock<IAccountService> accountService,
    Mock<IIngredientService> ingredientService)
    {
        return new AccountIngredientController(
            new Mock<ILogger<AccountIngredientController>>().Object,
            accountIngredientService.Object,
            accountService.Object,
            ingredientService.Object);
    }
}
