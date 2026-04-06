using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class AccountIngredientServiceTests
{
    private AccountIngredientService service;
    private Mock<IAccountIngredientRepo> mockRepo;
    private Mock<ITransactionManager> mockTransactionManager;

    [SetUp]
    public void Setup()
    {
        mockRepo = new Mock<IAccountIngredientRepo>();
        mockTransactionManager = new Mock<ITransactionManager>();
        mockTransactionManager.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work()).Verifiable();
        service = new AccountIngredientService(mockRepo.Object, mockTransactionManager.Object);
    }

    [Test]
    public async Task GetAccountIngredientByIds_CallsRepo()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var expected = new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 };

        mockRepo.Setup(s => s.GetAccountIngredient(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(expected).Verifiable();
        
        // Act
        var result = await service.GetAccountIngredientByIds(accountId, ingredientId);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetAccountIngredients_CallsRepo()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var expected = new List<AccountIngredient> { new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 } };

        mockRepo.Setup(s => s.GetAccountIngredients()).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccountIngredients();

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetAccountIngredientsByAccountId_CallsRepo()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var expected = new List<AccountIngredient> { new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 } };

        mockRepo.Setup(s => s.GetAccountIngredientsByAccountId(It.IsAny<Guid>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccountIngredientsByAccountId(accountId);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetAccountIngredientsByIngredientId_CallsRepo()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var expected = new List<AccountIngredient> { new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 } };

        mockRepo.Setup(s => s.GetAccountIngredientsByIngredientId(It.IsAny<Guid>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccountIngredientsByIngredientId(ingredientId);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task CreateAccountIngredient_CallsRepoWithTransaction()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var accountIngredient = new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 };

        mockRepo.Setup(s => s.CreateAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.CreateAccountIngredient(accountIngredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task UpdateAccountIngredient_CallsRepoWithTransaction()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var accountIngredient = new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 };

        mockRepo.Setup(s => s.UpdateAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.UpdateAccountIngredient(accountIngredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task DeleteAccountIngredient_CallsRepoWithTransaction()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var accountIngredient = new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 };

        mockRepo.Setup(s => s.DeleteAccountIngredient(It.IsAny<AccountIngredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.DeleteAccountIngredient(accountIngredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }
}
