namespace MealAssistant.Tests.DTO;

public class AccountResponseTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        var now = DateTime.UtcNow;
        var dto = new AccountResponse
        {
            Id = Guid.NewGuid(),
            FirstName = "F",
            LastName = "L",
            Email = "e@test.com",
            Username = "user",
        };

        Assert.That(dto.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(dto.FirstName, Is.EqualTo("F"));
        Assert.That(dto.LastName, Is.EqualTo("L"));
        Assert.That(dto.Email, Is.EqualTo("e@test.com"));
        Assert.That(dto.Username, Is.EqualTo("user"));
    }
}
