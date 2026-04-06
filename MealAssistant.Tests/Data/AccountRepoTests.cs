using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class AccountRepoTests
{
    private AppDbContext context;
    private AccountRepo accountRepo;
    private TransactionManager transactionManager;

    [SetUp]
    public void Setup()
    {
        context = TestDbContextFactory.Create();
        accountRepo = new AccountRepo(context);
        transactionManager = new TransactionManager(context);
    }

    [TearDown]
    public void Teardown()
    {
        context.Dispose();
    }

    [Test]
    public async Task CreateAccount_SetsIdAndDates_WhenMissing()
    {
        // Arrange
        var account = new Account { Username = "User" };

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await accountRepo.CreateAccount(account);
        });

        // Assert
        Assert.That(account.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(account.CreatedOn, Is.Not.EqualTo(default(DateTime)));
        Assert.That(account.LastLoggedIn, Is.Not.EqualTo(default(DateTime)));

        var createdAccount = await accountRepo.GetAccountById(account.Id);
        Assert.That(createdAccount, Is.Not.Null);
    }

    [Test]
    public async Task CreateAccount_KeepsIdsAndDates_WhenProvided()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var account = new Account 
        {
            Id = Guid.NewGuid(),
            Username = "User",
            CreatedOn = now,
            LastLoggedIn = now
        };

        // Act

        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await accountRepo.CreateAccount(account);
        });

        // Assert
        Assert.That(account.Id, Is.EqualTo(account.Id));
        Assert.That(account.CreatedOn, Is.EqualTo(account.CreatedOn));
        Assert.That(account.LastLoggedIn, Is.EqualTo(account.LastLoggedIn));

        var createdAccount = await accountRepo.GetAccountById(account.Id);
        Assert.That(createdAccount, Is.Not.Null);
    }

    [Test]
    public async Task GetAccount_IsCaseInsensitive()
    {
        // Arrange
        await context.Accounts.AddAsync(new Account { Username = "User" });
        await context.SaveChangesAsync();

        // Act
        var result = await accountRepo.GetAccount("uSeR");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Username, Is.EqualTo("User"));
    }

    [Test]
    public async Task GetAccountById_ReturnsMatchingRow()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid(), Username = "Test" };
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        // Act
        var result = await accountRepo.GetAccountById(account.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(account.Id));
    }

    [Test]
    public async Task GetAccounts_ReturnsAccounts()
    {
        // Arrange
        await context.Accounts.AddAsync(new Account { Id = Guid.NewGuid(), Username = "Test1" });
        await context.Accounts.AddAsync(new Account { Id = Guid.NewGuid(), Username = "Test2" });
        await context.Accounts.AddAsync(new Account { Id = Guid.NewGuid(), Username = "Test3" });
        await context.SaveChangesAsync();

        // Act
        var result = await accountRepo.GetAccounts();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task UpdateAccount_UpdatesSuccessfully()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid(), Username = "before" };
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        account.Username = "after";

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await accountRepo.UpdateAccount(account);
        });

        // Assert
        var updatedAccount = await accountRepo.GetAccountById(account.Id);
        Assert.That(updatedAccount, Is.Not.Null);
        Assert.That(updatedAccount.Username, Is.EqualTo("after"));
    }

    [Test]
    public async Task DeleteAccount_DeletesSuccessfully()
    {
        // Arrange
        var account = new Account { Id = Guid.NewGuid(), Username = "test" };
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await accountRepo.DeleteAccount(account);
        });

        // Assert
        var deletedAccount = await accountRepo.GetAccountById(account.Id);
        Assert.That(deletedAccount, Is.Null);
    }
}
