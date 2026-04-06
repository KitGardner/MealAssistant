using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Tests.Data;

public class AccountIngredientRepoTests
{
    private AccountIngredientRepo repo;
    private AppDbContext context;
    private ITransactionManager transactionManager;

    [SetUp]
    public void Setup()
    {
        context = TestDbContextFactory.Create();
        repo = new AccountIngredientRepo(context);
        transactionManager = new TransactionManager(context);
    }

    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }
    
    [Test]
    public async Task GetAccountIngredient_ReturnsMatchingRow()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 2
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAccountIngredient(accountId, ingredientId);


        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccountId, Is.EqualTo(accountId));
        Assert.That(result.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(result!.Amount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAccountIngredients_ReturnsAccountIngredients()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var ingredientId = Guid.NewGuid();
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = ingredientId,
            Amount = 2
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAccountIngredient(accountId, ingredientId);


        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccountId, Is.EqualTo(accountId));
        Assert.That(result.IngredientId, Is.EqualTo(ingredientId));
        Assert.That(result!.Amount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAccountIngredient_NoMatch_ReturnsNothing()
    {
        // Arrange
        var accountA = Guid.NewGuid();
        var accountB = Guid.NewGuid();
        var ingredientA = Guid.NewGuid();
        var ingredientB = Guid.NewGuid();

        await context.AccountIngredients.AddRangeAsync(
            new AccountIngredient { AccountId = accountA, IngredientId = ingredientA, Amount = 1 },
            new AccountIngredient { AccountId = accountA, IngredientId = ingredientB, Amount = 2 },
            new AccountIngredient { AccountId = accountB, IngredientId = ingredientA, Amount = 3 });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAccountIngredients();


        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetAccountIngredientsByAccountId_ReturnsRowsForAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = Guid.NewGuid(),
            Amount = 2
        });
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = accountId,
            IngredientId = Guid.NewGuid(),
            Amount = 2
        });
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 2
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAccountIngredientsByAccountId(accountId);


        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(r => r.AccountId == accountId), Is.True);
    }

    [Test]
    public async Task GetAccountIngredientsByIngredientId_ReturnsRowsForIngredient()
    {
        // Arrange
        var ingredientId = Guid.NewGuid();
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = ingredientId,
            Amount = 2
        });
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = ingredientId,
            Amount = 2
        });
        await context.AccountIngredients.AddAsync(new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 2
        });
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetAccountIngredientsByIngredientId(ingredientId);


        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(r => r.IngredientId == ingredientId), Is.True);
    }

    [Test]
    public async Task CreateAccountIngredient_CreatesSuccessfully()
    {
        // Arrange
        var accountIngredient = new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 2
        };

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.CreateAccountIngredient(accountIngredient);
        });
        

        // Assert
        var createdAccountIngredient = await repo.GetAccountIngredient(accountIngredient.AccountId, accountIngredient.IngredientId);
        Assert.That(createdAccountIngredient, Is.Not.Null);
    }

    [Test]
    public async Task UpdateAccountIngredient_UpdatesSuccessfully()
    {
        // Arrange
        var accountIngredient = new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 2
        };
        await context.AccountIngredients.AddAsync(accountIngredient);
        await context.SaveChangesAsync();

        accountIngredient.Amount = 5;

        // Act

        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.UpdateAccountIngredient(accountIngredient);
        });


        // Assert
        var updatedAccountIngredient = await repo.GetAccountIngredient(accountIngredient.AccountId, accountIngredient.IngredientId);
        Assert.That(updatedAccountIngredient, Is.Not.Null);
        Assert.That(updatedAccountIngredient.Amount, Is.EqualTo(5));
    }


    [Test]
    public async Task DeleteAccountIngredient_DeletesSuccessfully()
    {
        // Arrange
        var accountIngredient = new AccountIngredient
        {
            AccountId = Guid.NewGuid(),
            IngredientId = Guid.NewGuid(),
            Amount = 2
        };
        await context.AccountIngredients.AddAsync(accountIngredient);

        // Act
        await transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
        {
            await repo.DeleteAccountIngredient(accountIngredient);
        });


        // Assert
        var deletedAccountIngredient = await repo.GetAccountIngredient(accountIngredient.AccountId, accountIngredient.IngredientId);
        Assert.That(deletedAccountIngredient, Is.Null);
    }
}
