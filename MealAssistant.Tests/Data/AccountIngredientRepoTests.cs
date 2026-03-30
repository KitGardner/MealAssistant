using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class AccountIngredientRepoTests
{
    [Test]
    public async Task GetAccountIngredient_ReturnsMatchingJoinRow()
    {
        using var context = TestDbContextFactory.Create();
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 2
        });
        await context.SaveChangesAsync();
        var repo = new AccountIngredientRepo(context);

        var result = await repo.GetAccountIngredient(accountId, ingredientId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Amount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetByAccountAndIngredientId_FiltersCorrectly()
    {
        using var context = TestDbContextFactory.Create();
        var accountA = Guid.NewGuid();
        var accountB = Guid.NewGuid();
        var ingredientA = Guid.NewGuid();
        var ingredientB = Guid.NewGuid();
        await context.AccountIngredients.AddRangeAsync(
            new AccountIngredient { AccountId = accountA, IngredientId = ingredientA, Amount = 1 },
            new AccountIngredient { AccountId = accountA, IngredientId = ingredientB, Amount = 2 },
            new AccountIngredient { AccountId = accountB, IngredientId = ingredientA, Amount = 3 });
        await context.SaveChangesAsync();
        var repo = new AccountIngredientRepo(context);

        var byAccount = await repo.GetAccountIngredientsByAccountId(accountA);
        var byIngredient = await repo.GetAccountIngredientsByIngredientId(ingredientA);

        Assert.That(byAccount.Count, Is.EqualTo(2));
        Assert.That(byIngredient.Count, Is.EqualTo(2));
    }
}
