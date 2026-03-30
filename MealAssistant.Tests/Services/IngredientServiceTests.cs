using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class IngredientServiceTests
{
    [Test]
    public async Task Getters_DelegateToRepository()
    {
        var repo = new Mock<IIngredientRepo>();
        var tx = new Mock<ITransactionManager>();
        var expected = new Ingredient { Name = "salt" };
        repo.Setup(r => r.GetIngredient("salt")).ReturnsAsync(expected);
        repo.Setup(r => r.GetIngredientById(It.IsAny<Guid>())).ReturnsAsync(expected);
        repo.Setup(r => r.GetIngredients()).ReturnsAsync(new List<Ingredient> { expected });
        var sut = new IngredientService(repo.Object, tx.Object);

        var byName = await sut.GetIngredient("salt");
        var byId = await sut.GetIngredientById(Guid.NewGuid());
        var all = await sut.GetIngredients();

        Assert.That(byName, Is.SameAs(expected));
        Assert.That(byId, Is.SameAs(expected));
        Assert.That(all.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Mutations_ExecuteWithinTransactionManager()
    {
        var repo = new Mock<IIngredientRepo>();
        var tx = new Mock<ITransactionManager>();
        tx.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work());
        var sut = new IngredientService(repo.Object, tx.Object);
        var ingredient = new Ingredient();

        await sut.CreateIngredient(ingredient);
        await sut.UpdateIngredient(ingredient);
        await sut.DeleteIngredient(ingredient);

        tx.Verify(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()), Times.Exactly(3));
        repo.Verify(r => r.CreateIngredient(ingredient), Times.Once);
        repo.Verify(r => r.UpdateIngredient(ingredient), Times.Once);
        repo.Verify(r => r.DeleteIngredient(ingredient), Times.Once);
    }
}
