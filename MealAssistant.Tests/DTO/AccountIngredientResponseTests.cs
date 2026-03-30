namespace MealAssistant.Tests.DTO;

public class AccountIngredientResponseTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        var dto = new AccountIngredientResponse
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 3
        };

        Assert.That(dto.AccountId, Is.EqualTo(accountId));
        Assert.That(dto.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(dto.Amount, Is.EqualTo(3));
    }
}
