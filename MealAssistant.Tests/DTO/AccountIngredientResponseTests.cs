namespace MealAssistant.Tests.DTO;

public class AccountIngredientResponseTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        
        // Act
        var dto = new AccountIngredientResponse
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 3
        };

        // Assert
        Assert.That(dto.AccountId, Is.EqualTo(accountId));
        Assert.That(dto.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(dto.Amount, Is.EqualTo(3));
    }
}
