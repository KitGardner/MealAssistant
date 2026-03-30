using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class AccountIngredientServiceTests
{
    [Test]
    public async Task Getters_DelegateToRepository()
    {
        var repo = new Mock<IAccountIngredientRepo>();
        var tx = new Mock<ITransactionManager>();
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var expected = new AccountIngredient { AccountId = accountId, IngredientId = ingredientId, Amount = 4 };
        repo.Setup(r => r.GetAccountIngredient(accountId, ingredientId)).ReturnsAsync(expected);
        repo.Setup(r => r.GetAccountIngredients()).ReturnsAsync(new List<AccountIngredient> { expected });
        repo.Setup(r => r.GetAccountIngredientsByAccountId(accountId)).ReturnsAsync(new List<AccountIngredient> { expected });
        repo.Setup(r => r.GetAccountIngredientsByIngredientId(ingredientId)).ReturnsAsync(new List<AccountIngredient> { expected });
        var sut = new AccountIngredientService(repo.Object, tx.Object);

        Assert.That(await sut.GetAccountIngredientByIds(accountId, ingredientId), Is.SameAs(expected));
        Assert.That((await sut.GetAccountIngredients()).Count, Is.EqualTo(1));
        Assert.That((await sut.GetAccountIngredientsByAccountId(accountId)).Count, Is.EqualTo(1));
        Assert.That((await sut.GetAccountIngredientsByIngredientId(ingredientId)).Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Mutations_ExecuteWithinTransactionManager()
    {
        var repo = new Mock<IAccountIngredientRepo>();
        var tx = new Mock<ITransactionManager>();
        tx.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work());
        var sut = new AccountIngredientService(repo.Object, tx.Object);
        var row = new AccountIngredient();

        await sut.CreateAccountIngredient(row);
        await sut.UpdateAccountIngredient(row);
        await sut.DeleteAccountIngredient(row);

        tx.Verify(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()), Times.Exactly(3));
        repo.Verify(r => r.CreateAccountIngredient(row), Times.Once);
        repo.Verify(r => r.UpdateAccountIngredient(row), Times.Once);
        repo.Verify(r => r.DeleteAccountIngredient(row), Times.Once);
    }
}
