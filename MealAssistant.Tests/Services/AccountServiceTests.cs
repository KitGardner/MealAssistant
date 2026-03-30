using MealAssistant.Data;
using MealAssistant.Objects;
using MealAssistant.Services;
using Moq;

namespace MealAssistant.Tests.Services;

public class AccountServiceTests
{
    [Test]
    public async Task Getters_DelegateToRepository()
    {
        var repo = new Mock<IAccountRepo>();
        var tx = new Mock<ITransactionManager>();
        var expected = new Account { Username = "kit" };
        repo.Setup(r => r.GetAccount("kit")).ReturnsAsync(expected);
        repo.Setup(r => r.GetAccountById(It.IsAny<Guid>())).ReturnsAsync(expected);
        repo.Setup(r => r.GetAccounts()).ReturnsAsync(new List<Account> { expected });
        var sut = new AccountService(repo.Object, tx.Object);

        var byName = await sut.GetAccount("kit");
        var byId = await sut.GetAccountById(Guid.NewGuid());
        var all = await sut.GetAccounts();

        Assert.That(byName, Is.SameAs(expected));
        Assert.That(byId, Is.SameAs(expected));
        Assert.That(all.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task Mutations_ExecuteWithinTransactionManager()
    {
        var repo = new Mock<IAccountRepo>();
        var tx = new Mock<ITransactionManager>();
        tx.Setup(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()))
            .Returns((Func<Task> work) => work());
        var sut = new AccountService(repo.Object, tx.Object);
        var account = new Account();

        await sut.CreateAccount(account);
        await sut.UpdateAccount(account);
        await sut.DeleteAccount(account);

        tx.Verify(t => t.ExecuteInTransactionWithSaveAsync(It.IsAny<Func<Task>>()), Times.Exactly(3));
        repo.Verify(r => r.CreateAccount(account), Times.Once);
        repo.Verify(r => r.UpdateAccount(account), Times.Once);
        repo.Verify(r => r.DeleteAccount(account), Times.Once);
    }
}
