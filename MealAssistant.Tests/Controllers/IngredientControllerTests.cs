using MealAssistant.Controllers;
using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MealAssistant.Tests.Controllers;

public class IngredientControllerTests
{
    private static IngredientController CreateController(Mock<IIngredientService> service)
    {
        return new IngredientController(new Mock<ILogger<IngredientController>>().Object, service.Object);
    }

    [Test]
    public async Task GetIngredient_WhenMissing_ReturnsNotFound()
    {
        var service = new Mock<IIngredientService>();
        service.Setup(s => s.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync((Ingredient?)null);
        var controller = CreateController(service);

        var result = await controller.GetIngredient(Guid.NewGuid());

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task CreateIngredient_ReturnsCreatedAtAction()
    {
        var service = new Mock<IIngredientService>();
        service.Setup(s => s.CreateIngredient(It.IsAny<Ingredient>()))
            .Callback<Ingredient>(i => i.Id = Guid.NewGuid())
            .Returns(Task.CompletedTask);
        var controller = CreateController(service);

        var result = await controller.CreateIngredient(new IngredientRequest { Name = "Salt" });

        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task UpdateIngredient_WhenBodyIdMismatch_ReturnsBadRequest()
    {
        var service = new Mock<IIngredientService>();
        var controller = CreateController(service);

        var result = await controller.UpdateIngredient(Guid.NewGuid(), new IngredientRequest { Id = Guid.NewGuid() });

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }
}
