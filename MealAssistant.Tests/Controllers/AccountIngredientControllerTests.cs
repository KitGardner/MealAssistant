using MealAssistant.Controllers;
using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MealAssistant.Tests.Controllers;

public class AccountIngredientControllerTests
{
    private static AccountIngredientController CreateController(
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

    [Test]
    public async Task GetAccountIngredient_WhenMissing_ReturnsNotFound()
    {
        var aiService = new Mock<IAccountIngredientService>();
        var accountService = new Mock<IAccountService>();
        var ingredientService = new Mock<IIngredientService>();
        aiService.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((AccountIngredient?)null);
        var controller = CreateController(aiService, accountService, ingredientService);

        var result = await controller.GetAccountIngredient(Guid.NewGuid(), Guid.NewGuid());

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task CreateAccountIngredient_WhenParentMissing_ReturnsBadRequest()
    {
        var aiService = new Mock<IAccountIngredientService>();
        var accountService = new Mock<IAccountService>();
        var ingredientService = new Mock<IIngredientService>();
        accountService.Setup(s => s.GetAccountById(It.IsAny<Guid>())).ReturnsAsync((Account?)null);
        var controller = CreateController(aiService, accountService, ingredientService);

        var result = await controller.CreateAccountIngredient(new AccountIngredientRequest
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 1
        });

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task DeleteAccountIngredient_WhenFound_ReturnsNoContent()
    {
        var aiService = new Mock<IAccountIngredientService>();
        var accountService = new Mock<IAccountService>();
        var ingredientService = new Mock<IIngredientService>();
        aiService.Setup(s => s.GetAccountIngredientByIds(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(new AccountIngredient());
        var controller = CreateController(aiService, accountService, ingredientService);

        var result = await controller.DeleteAccountIngredient(Guid.NewGuid(), Guid.NewGuid());

        Assert.That(result, Is.TypeOf<NoContentResult>());
        aiService.Verify(s => s.DeleteAccountIngredient(It.IsAny<AccountIngredient>()), Times.Once);
    }
}
