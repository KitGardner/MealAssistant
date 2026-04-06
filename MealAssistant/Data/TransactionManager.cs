
namespace MealAssistant.Data
{
    public interface ITransactionManager
    {
        Task ExecuteInTransactionWithSaveAsync(Func<Task> work);
    }


    public class TransactionManager : ITransactionManager
    {
        private readonly AppDbContext _dbContext;

        public TransactionManager(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task ExecuteInTransactionWithSaveAsync(Func<Task> work)
        {
            if (work == null) throw new ArgumentNullException(nameof(work));

            await using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await work();
                await _dbContext.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}

