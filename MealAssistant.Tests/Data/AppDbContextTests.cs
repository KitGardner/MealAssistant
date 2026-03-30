using MealAssistant.Data;
using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Tests.Data;

public class AppDbContextTests
{
    [Test]
    public void Model_ConfiguresCompositeKey_ForAccountIngredient()
    {
        using var context = TestDbContextFactory.Create();
        var entityType = context.Model.FindEntityType(typeof(AccountIngredient));

        var keyProps = entityType!.FindPrimaryKey()!.Properties.Select(p => p.Name).ToArray();
        Assert.That(keyProps, Is.EqualTo(new[] { nameof(AccountIngredient.AccountId), nameof(AccountIngredient.IngredientId) }));
    }

    [Test]
    public void Model_ConfiguresCascadeDelete_ForBothRelationships()
    {
        using var context = TestDbContextFactory.Create();
        var entityType = context.Model.FindEntityType(typeof(AccountIngredient))!;

        var accountFk = entityType.GetForeignKeys().Single(f => f.Properties.Single().Name == nameof(AccountIngredient.AccountId));
        var ingredientFk = entityType.GetForeignKeys().Single(f => f.Properties.Single().Name == nameof(AccountIngredient.IngredientId));

        Assert.That(accountFk.DeleteBehavior, Is.EqualTo(DeleteBehavior.Cascade));
        Assert.That(ingredientFk.DeleteBehavior, Is.EqualTo(DeleteBehavior.Cascade));
    }
}
