namespace MealAssistant.Tests.DTO;

public class AccountIngredientRequestTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();

        // Act
        var dto = new AccountIngredientRequest
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 7
        };

        // Assert
        Assert.That(dto.AccountId, Is.EqualTo(accountId));
        Assert.That(dto.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(dto.Amount, Is.EqualTo(7));
    }
}
