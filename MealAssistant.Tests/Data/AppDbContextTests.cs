using MealAssistant.Data;
using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Tests.Data;

public class AppDbContextTests
{
    private AppDbContext context;

    [SetUp]
    public void Setup()
    {
        context = TestDbContextFactory.Create();
    }

    [TearDown]
    public void Teardown()
    {
        context.Dispose();
    }

    [Test]
    public void Model_ConfiguresCompositeKey_ForAccountIngredient()
    {
        // Arrange
        var entityType = context.Model.FindEntityType(typeof(AccountIngredient));

        // Act
        var keyProps = entityType!.FindPrimaryKey()!.Properties.Select(p => p.Name).ToArray();
        
        // Assert
        Assert.That(keyProps, Is.EqualTo(new[] { nameof(AccountIngredient.AccountId), nameof(AccountIngredient.IngredientId) }));
    }

    [Test]
    public void Model_ConfiguresCascadeDelete_ForBothRelationships()
    {
        // Arrange
        var entityType = context.Model.FindEntityType(typeof(AccountIngredient))!;

        // Act
        var accountFk = entityType.GetForeignKeys().Single(f => f.Properties.Single().Name == nameof(AccountIngredient.AccountId));
        var ingredientFk = entityType.GetForeignKeys().Single(f => f.Properties.Single().Name == nameof(AccountIngredient.IngredientId));

        // Assert
        Assert.That(accountFk.DeleteBehavior, Is.EqualTo(DeleteBehavior.Cascade));
        Assert.That(ingredientFk.DeleteBehavior, Is.EqualTo(DeleteBehavior.Cascade));
    }
}
