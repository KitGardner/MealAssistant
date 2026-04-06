using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class IngredientRepoTests
{
    private IngredientRepo repo;
    private AppDbContext context;
    private ITransactionManager transactionManager;

    [SetUp]
    public void Setup()
    {
        context = TestDbContextFactory.Create();
        repo = new IngredientRepo(context);
        transactionManager = new TransactionManager(context);
    }

    [TearDown]
    public void Teardown()
    {
        context.Dispose();
    }

    [Test]
    public async Task CreateIngredient_SetsDefaults_WhenMissing()
    {
        // Arrange
        var ingredient = new Ingredient { Name = "Salt" };

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.CreateIngredient(ingredient);
        });

        // Assert
        Assert.That(ingredient.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(ingredient.CreatedOn, Is.Not.EqualTo(default(DateTime)));
        Assert.That(ingredient.LastUpdatedOn, Is.Not.EqualTo(default(DateTime)));

        var createdIngredient = await repo.GetIngredientById(ingredient.Id);
        Assert.That(createdIngredient, Is.Not.Null);
    }

    [Test]
    public async Task CreateIngredient_ValuesProvided_KeepsValues()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var id = Guid.NewGuid();
        var ingredient = new Ingredient { Id = id, Name = "Salt", CreatedOn = now };

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.CreateIngredient(ingredient);
        });

        // Assert
        Assert.That(ingredient.Id, Is.EqualTo(id));
        Assert.That(ingredient.CreatedOn, Is.EqualTo(now));
        Assert.That(ingredient.LastUpdatedOn, Is.Not.EqualTo(default(DateTime)));

        var createdIngredient = await repo.GetIngredientById(ingredient.Id);
        Assert.That(createdIngredient, Is.Not.Null);
    }

    [Test]
    public async Task GetIngredient_IsCaseInsensitive()
    {
        // Arrange
        await context.Ingredients.AddAsync(new Ingredient { Name = "Sugar" });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetIngredient("sUgAr");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Sugar"));
    }

    [Test]
    public async Task GetIngredientById_returnsMatchingIngredient()
    {
        // Arrange
        var id = Guid.NewGuid();
        await context.Ingredients.AddAsync(new Ingredient { Id = id, Name = "Sugar" });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetIngredientById(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetIngredients_GetsAll()
    {
        // Arrange
        await context.Ingredients.AddAsync(new Ingredient { Name = "Sugar" });
        await context.Ingredients.AddAsync(new Ingredient { Name = "Cinnamon" });
        await context.Ingredients.AddAsync(new Ingredient { Name = "Salt" });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetIngredients();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task UpdateIngredient_UpdatesSuccessfully()
    {
        // Arrange
        var ingredient = new Ingredient {Id = Guid.NewGuid(), Name = "before" };
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();

        ingredient.Name = "after";

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.UpdateIngredient(ingredient);
        });

        // Assert
        var updatedIngredient = await repo.GetIngredientById(ingredient.Id);

        Assert.That(updatedIngredient, Is.Not.Null);
        Assert.That(updatedIngredient.Name, Is.EqualTo("after"));
    }

    [Test]
    public async Task DeleteIngredient_DeletesSuccessfully()
    {
        // Arrange
        var ingredient = new Ingredient { Id = Guid.NewGuid(), Name = "Sugar" };
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.DeleteIngredient(ingredient);
        });

        // Assert
        var deletedIngredient = await repo.GetIngredientById(ingredient.Id);

        Assert.That(deletedIngredient, Is.Null);
    }
}
