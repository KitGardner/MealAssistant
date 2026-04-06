using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class TransactionManagerTests
{
    private AppDbContext context;
    private TransactionManager transactionManager;

    [SetUp]
    public void Setup()
    {
        context = TestDbContextFactory.Create();
        transactionManager = new TransactionManager(context);
    }

    [TearDown]
    public void Teardown()
    {
        context.Dispose();
    }

    [Test]
    public void Constructor_NullContext_Throws()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new TransactionManager(null!));
    }

    [Test]
    public void ExecuteInTransactionWithSaveAsync_NullWork_Throws()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await transactionManager.ExecuteInTransactionWithSaveAsync(null!));
    }

    [Test]
    public async Task ExecuteInTransactionWithSaveAsync_SavesChanges()
    {
        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await context.Ingredients.AddAsync(new Ingredient { Name = "Salt", Description = "Fine" });
        });

        // Assert
        Assert.That(context.Ingredients.Count(), Is.EqualTo(1));
    }
}
