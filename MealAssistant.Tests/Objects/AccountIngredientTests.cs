using MealAssistant.Objects;

namespace MealAssistant.Tests.Objects;

public class AccountIngredientTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();

        // Act
        var accountIngredient = new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 12
        };

        // Assert
        Assert.That(accountIngredient.AccountId, Is.EqualTo(accountId));
        Assert.That(accountIngredient.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(accountIngredient.Amount, Is.EqualTo(12));
    }
}
