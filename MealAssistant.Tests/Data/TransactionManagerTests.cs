using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class TransactionManagerTests
{
    [Test]
    public void Constructor_NullContext_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TransactionManager(null!));
    }

    [Test]
    public void ExecuteInTransactionWithSaveAsync_NullWork_Throws()
    {
        using var context = TestDbContextFactory.Create();
        var manager = new TransactionManager(context);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await manager.ExecuteInTransactionWithSaveAsync(null!));
    }

    [Test]
    public async Task ExecuteInTransactionWithSaveAsync_SavesChanges()
    {
        using var context = TestDbContextFactory.Create();
        var manager = new TransactionManager(context);

        await manager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await context.Ingredients.AddAsync(new Ingredient { Name = "Salt", Description = "Fine" });
        });

        Assert.That(context.Ingredients.Count(), Is.EqualTo(1));
    }
}
