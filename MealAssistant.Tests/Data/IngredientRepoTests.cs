using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class IngredientRepoTests
{
    [Test]
    public async Task CreateIngredient_SetsDefaults_WhenMissing()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new IngredientRepo(context);
        var ingredient = new Ingredient { Name = "Salt" };

        await repo.CreateIngredient(ingredient);
        await context.SaveChangesAsync();

        Assert.That(ingredient.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(ingredient.CreatedOn, Is.Not.EqualTo(default(DateTime)));
        Assert.That(ingredient.LastUpdatedOn, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public async Task GetIngredient_IsCaseInsensitive()
    {
        using var context = TestDbContextFactory.Create();
        await context.Ingredients.AddAsync(new Ingredient { Name = "Sugar" });
        await context.SaveChangesAsync();
        var repo = new IngredientRepo(context);

        var result = await repo.GetIngredient("sUgAr");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Sugar"));
    }

    [Test]
    public async Task UpdateAndDelete_ApplyChangesAfterSave()
    {
        using var context = TestDbContextFactory.Create();
        var ingredient = new Ingredient { Name = "before" };
        await context.Ingredients.AddAsync(ingredient);
        await context.SaveChangesAsync();
        var repo = new IngredientRepo(context);

        ingredient.Name = "after";
        await repo.UpdateIngredient(ingredient);
        await context.SaveChangesAsync();
        Assert.That((await repo.GetIngredientById(ingredient.Id))!.Name, Is.EqualTo("after"));

        await repo.DeleteIngredient(ingredient);
        await context.SaveChangesAsync();
        Assert.That(await repo.GetIngredientById(ingredient.Id), Is.Null);
    }
}
