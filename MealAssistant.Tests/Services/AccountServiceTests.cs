using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class AccountServiceTests
{
    private AccountService service;
    private Mock<IAccountRepo> mockRepo;
    private Mock<ITransactionManager> mockTransactionManager;

    [SetUp]
    public void Setup()
    {
        mockRepo = new Mock<IAccountRepo>();
        mockTransactionManager = new Mock<ITransactionManager>();
        mockTransactionManager.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work()).Verifiable();
        service = new AccountService(mockRepo.Object, mockTransactionManager.Object);
    }

    [Test]
    public async Task GetAccount_CallsRepo()
    {
        // Arrange
        var expected = new Account { Username = "Test" };
        mockRepo.Setup(r => r.GetAccount(It.IsAny<string>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccount("Test");

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetAccountById_CallsRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new Account { Id = id, Username = "Test" };
        mockRepo.Setup(r => r.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccountById(id);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetAccounts_CallsRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new List<Account> { new Account { Id = id, Username = "Test" } };
        mockRepo.Setup(r => r.GetAccounts()).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetAccounts();

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task CreateAccount_CallsRepo()
    {
        // Arrange
        var account = new Account { Username = "Test" };
        mockRepo.Setup(s => s.CreateAccount(It.IsAny<Account>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.CreateAccount(account);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task UpdateAccount_CallsRepo()
    {
        // Arrange
        var account = new Account { Username = "Test" };
        mockRepo.Setup(s => s.UpdateAccount(It.IsAny<Account>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.UpdateAccount(account);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task DeleteAccount_CallsRepo()
    {
        // Arrange
        var account = new Account { Username = "Test" };
        mockRepo.Setup(s => s.DeleteAccount(It.IsAny<Account>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.DeleteAccount(account);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }
}
