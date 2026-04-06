namespace MealAssistant.Tests.DTO;

public class AccountRequestTests
{
    [Test]
    public void Properties_RoundTripValues()
    {
        // Arrange
        var now = DateTime.UtcNow;
        
        // Act
        var dto = new AccountRequest
        {
            Id = Guid.NewGuid(),
            FirstName = "F",
            LastName = "L",
            Email = "e@test.com",
            Username = "user",
            Password = "pw"
        };

        // Assert
        Assert.That(dto.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(dto.FirstName, Is.EqualTo("F"));
        Assert.That(dto.LastName, Is.EqualTo("L"));
        Assert.That(dto.Email, Is.EqualTo("e@test.com"));
        Assert.That(dto.Username, Is.EqualTo("user"));
        Assert.That(dto.Password, Is.EqualTo("pw"));
    }
}
