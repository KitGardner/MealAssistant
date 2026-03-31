namespace MealAssistant.Tests.DTO;

public class IngredientRequestTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        var dto = new IngredientRequest
        {
            Id = Guid.NewGuid(),
            Name = "Salt",
            Description = "Fine"
        };

        Assert.That(dto.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(dto.Name, Is.EqualTo("Salt"));
        Assert.That(dto.Description, Is.EqualTo("Fine"));
    }
}
