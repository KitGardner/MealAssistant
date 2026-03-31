using MealAssistant.Controllers;
using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MealAssistant.Tests.Controllers;

public class IngredientControllerTests
{
    private IngredientController controller;
    private Mock<IIngredientService> service;

    [SetUp]
    public void Setup()
    {
        service = new Mock<IIngredientService>();
        controller = CreateController(service);
    }

    [Test]
    public async Task GetIngredients_NameNotSpecified_ReturnsAllIngredients()
    {
        // Arrange
        var ingredients = CreateTestIngredients(3);
        service.Setup(s => s.GetIngredients()).ReturnsAsync(ingredients);

        // Act
        var result = await controller.GetIngredients(null);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<IngredientResponse>>());

        var returned = response.Value as List<IngredientResponse>;
        Assert.That(returned.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetIngredients_NameSpecified_ReturnsNamedIngredient()
    {
        // Arrange
        var ingredient = CreateTestIngredients(1).First();
        service.Setup(s => s.GetIngredient(It.IsAny<string>())).ReturnsAsync(ingredient);

        // Act
        var result = await controller.GetIngredients(ingredient.Name);

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<List<IngredientResponse>>());

        var returned = response.Value as List<IngredientResponse>;
        Assert.That(returned[0].Name, Is.EqualTo(ingredient.Name));
    }

    [Test]
    public async Task GetIngredients_IngredientNotExist_ReturnsNotFound()
    {
        // Arrange
        service.Setup(s => s.GetIngredient(It.IsAny<string>())).ReturnsAsync((Ingredient?)null);

        // Act
        var result = await controller.GetIngredients("Invalid");

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetIngredient_WhenMissing_ReturnsNotFound()
    {
        // Arrange
        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);

        // Act
        var result = await controller.GetIngredient(Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetIngredient_ReturnsIngredient()
    {
        // Arrange
        var testIngredient = CreateTestIngredients(1).First();
        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(testIngredient);

        // Act
        var result = await controller.GetIngredient(Guid.NewGuid());

        // Assert
        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

        var response = result.Result as OkObjectResult;
        Assert.That(response.Value, Is.TypeOf<IngredientResponse>());
    }

    [Test]
    public async Task CreateIngredient_ReturnsCreatedAtAction()
    {
        // Arrange
        var ingredientRequest = new IngredientRequest
        {
            Name = "Salt",
            Description = "Test Description"
        };

        service.Setup(s => s.CreateIngredient(It.IsAny<Ingredient>()))
            .Callback<Ingredient>(i => i.Id = Guid.NewGuid())
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await controller.CreateIngredient(ingredientRequest);

        // Assert
        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());

        var response = result.Result as CreatedAtActionResult;
        Assert.That(response.Value, Is.TypeOf<IngredientResponse>());

        var createdIngredient = response.Value as IngredientResponse;
        Assert.That(createdIngredient.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(createdIngredient.Name, Is.EqualTo(ingredientRequest.Name));
        Assert.That(createdIngredient.Description, Is.EqualTo(ingredientRequest?.Description));
    }

    [Test]
    public async Task UpdateIngredient_NullPayload_ReturnsBadRequest()
    {
        // Act
        var result = await controller.UpdateIngredient(Guid.NewGuid(), null);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task UpdateIngredient_WhenBodyIdMismatch_ReturnsBadRequest()
    {
        // Act
        var result = await controller.UpdateIngredient(Guid.NewGuid(), new IngredientRequest { Id = Guid.NewGuid() });

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateIngredient_NoMatchingIngredient_CreatesNew()
    {
        // Arrange
        var ingredientRequest = new IngredientRequest
        {
            Name = "Salt",
            Description = "Test Description"
        };

        var id = Guid.NewGuid();

        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);
        service.Setup(s => s.CreateIngredient(It.IsAny<Ingredient>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateIngredient(id, ingredientRequest);

        // Assert
        Assert.That(result, Is.TypeOf<CreatedAtActionResult>());

        var response = result as CreatedAtActionResult;
        Assert.That(response.Value, Is.TypeOf<IngredientResponse>());

        var createdIngredient = response.Value as IngredientResponse;
        Assert.That(createdIngredient.Id, Is.EqualTo(id));
        Assert.That(createdIngredient.Name, Is.EqualTo(ingredientRequest.Name));
        Assert.That(createdIngredient.Description, Is.EqualTo(ingredientRequest?.Description));
    }

    [Test]
    public async Task UpdateIngredient_MatchingIngredient_Updates()
    {
        // Arrange
        var id = Guid.NewGuid();

        var ingredientRequest = new IngredientRequest
        {
            Id = id,
            Name = "Salt",
            Description = "Test Description"
        };

        var testIngredient = CreateTestIngredients(1).First();
        testIngredient.Id = id;

        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(testIngredient);
        service.Setup(s => s.UpdateIngredient(It.IsAny<Ingredient>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateIngredient(id, ingredientRequest);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteIngredient_NoMatchingIngredient_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);

        // Act
        var result = await controller.DeleteIngredient(id);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteIngredient_MatchingIngredient_ReturnsNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testIngredient = CreateTestIngredients(1).First();

        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(testIngredient);
        service.Setup(s => s.DeleteIngredient(It.IsAny<Ingredient>())).Returns(Task.CompletedTask);

        // Act
        var result = await controller.DeleteIngredient(id);

        // Assert
        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    private List<Ingredient> CreateTestIngredients(int count)
    {
        var ingredients = new List<Ingredient>();

        for (int i = 0; i < count; i++)
        {
            ingredients.Add(new Ingredient
            {
                Name = $"Test Ingredient {i}",
                Description = "Test Description"
            });
        }

        return ingredients;
    }

    private IngredientController CreateController(Mock<IIngredientService> service)
    {
        return new IngredientController(new Mock<ILogger<IngredientController>>().Object, service.Object);
    }
}
