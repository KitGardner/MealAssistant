using MealAssistant.Objects;

namespace MealAssistant.Tests.Objects;

public class AccountTests
{
    [Test]
    public void Defaults_AreInitialized()
    {
        // Act
        var account = new Account();

        // Assert
        Assert.That(account.FirstName, Is.EqualTo(string.Empty));
        Assert.That(account.LastName, Is.EqualTo(string.Empty));
        Assert.That(account.Email, Is.EqualTo(string.Empty));
        Assert.That(account.Username, Is.EqualTo(string.Empty));
        Assert.That(account.Password, Is.EqualTo(string.Empty));
        Assert.That(account.AccountIngredients, Is.Not.Null);
        Assert.That(account.AccountIngredients, Is.Empty);
    }
}
