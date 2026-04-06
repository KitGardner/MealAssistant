namespace MealAssistant.Tests.DTO;

public class IngredientResponseTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        // Act
        var dto = new IngredientResponse
        {
            Id = Guid.NewGuid(),
            Name = "Salt",
            Description = "Fine"
        };

        // Assert
        Assert.That(dto.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(dto.Name, Is.EqualTo("Salt"));
        Assert.That(dto.Description, Is.EqualTo("Fine"));
    }
}
