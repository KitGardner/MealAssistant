using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class IngredientServiceTests
{
    private IngredientService service;
    private Mock<IIngredientRepo> mockRepo;
    private Mock<ITransactionManager> mockTransactionManager;


    [SetUp]
    public void Setup()
    {
        mockRepo = new Mock<IIngredientRepo>();
        mockTransactionManager = new Mock<ITransactionManager>();
        mockTransactionManager.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work()).Verifiable();
        service = new IngredientService(mockRepo.Object, mockTransactionManager.Object);
    }

    [Test]
    public async Task GetIngredient_CallsRepo()
    {
        // Arrange
        var expected = new Ingredient { Name = "Salt" };
        mockRepo.Setup(r => r.GetIngredient(It.IsAny<string>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetIngredient("Salt");

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetIngredientById_CallsRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new Ingredient { Id = id, Name = "Salt" };
        mockRepo.Setup(r => r.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetIngredientById(id);

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task GetIngredients_CallsRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new List<Ingredient> { new Ingredient { Id = id, Name = "Salt" } };
        mockRepo.Setup(r => r.GetIngredients()).ReturnsAsync(expected).Verifiable();

        // Act
        var result = await service.GetIngredients();

        // Assert
        Assert.That(result, Is.SameAs(expected));
        mockRepo.VerifyAll();
    }

    [Test]
    public async Task CreateIngredient_CallsRepo()
    {
        // Arrange
        var Ingredient = new Ingredient { Name = "Salt" };
        mockRepo.Setup(s => s.CreateIngredient(It.IsAny<Ingredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.CreateIngredient(Ingredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task UpdateIngredient_CallsRepo()
    {
        // Arrange
        var Ingredient = new Ingredient { Name = "Salt" };
        mockRepo.Setup(s => s.UpdateIngredient(It.IsAny<Ingredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.UpdateIngredient(Ingredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }

    [Test]
    public async Task DeleteIngredient_CallsRepo()
    {
        // Arrange
        var Ingredient = new Ingredient { Name = "Salt" };
        mockRepo.Setup(s => s.DeleteIngredient(It.IsAny<Ingredient>())).Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.DeleteIngredient(Ingredient);

        // Assert
        mockRepo.VerifyAll();
        mockTransactionManager.VerifyAll();
    }
}
