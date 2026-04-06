using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccount(string username);
        Task<Account?> GetAccountById(Guid id);
        Task<List<Account>> GetAccounts();
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(Account account);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _repository;
        private readonly ITransactionManager _transactionManager;

        public AccountService(IAccountRepo repo, ITransactionManager transactionManager)
        {
            _repository = repo;
            _transactionManager = transactionManager;
        }

        public async Task<Account?> GetAccount(string username)
        {
            return await _repository.GetAccount(username);
        }

        public async Task<Account?> GetAccountById(Guid id)
        {
            return await _repository.GetAccountById(id);
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await _repository.GetAccounts();
        }

        public async Task CreateAccount(Account account)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.CreateAccount(account);
            });
        }

        public async Task UpdateAccount(Account account)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.UpdateAccount(account);
            });
        }

        public async Task DeleteAccount(Account account)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.DeleteAccount(account);
            });
        }
    }
}

