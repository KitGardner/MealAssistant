using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class AccountRepoTests
{
    [Test]
    public async Task CreateAccount_SetsIdAndDates_WhenMissing()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new AccountRepo(context);
        var account = new Account { Username = "User" };

        await repo.CreateAccount(account);
        await context.SaveChangesAsync();

        Assert.That(account.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(account.CreatedOn, Is.Not.EqualTo(default(DateTime)));
        Assert.That(account.LastLoggedIn, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public async Task GetAccount_IsCaseInsensitive()
    {
        using var context = TestDbContextFactory.Create();
        await context.Accounts.AddAsync(new Account { Username = "Kit" });
        await context.SaveChangesAsync();
        var repo = new AccountRepo(context);

        var result = await repo.GetAccount("kIt");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Username, Is.EqualTo("Kit"));
    }

    [Test]
    public async Task UpdateAndDelete_ApplyChangesAfterSave()
    {
        using var context = TestDbContextFactory.Create();
        var account = new Account { Username = "before" };
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
        var repo = new AccountRepo(context);

        account.Username = "after";
        await repo.UpdateAccount(account);
        await context.SaveChangesAsync();
        Assert.That((await repo.GetAccountById(account.Id))!.Username, Is.EqualTo("after"));

        await repo.DeleteAccount(account);
        await context.SaveChangesAsync();
        Assert.That(await repo.GetAccountById(account.Id), Is.Null);
    }
}
