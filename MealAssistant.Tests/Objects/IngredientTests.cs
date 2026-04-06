using MealAssistant.Objects;

namespace MealAssistant.Tests.Objects;

public class IngredientTests
{
    [Test]
    public void Defaults_AreInitialized()
    {
        // Act
        var ingredient = new Ingredient();

        // Assert
        Assert.That(ingredient.Name, Is.EqualTo(string.Empty));
        Assert.That(ingredient.Description, Is.EqualTo(string.Empty));
        Assert.That(ingredient.AccountIngredients, Is.Not.Null);
        Assert.That(ingredient.AccountIngredients, Is.Empty);
    }
}
