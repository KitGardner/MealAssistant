using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Services
{
    public interface IAccountIngredientService
    {
        Task<AccountIngredient?> GetAccountIngredientByIds(Guid accountId, Guid ingredientId);
        Task<List<AccountIngredient>> GetAccountIngredients();
        Task<List<AccountIngredient>> GetAccountIngredientsByAccountId(Guid accountId);
        Task<List<AccountIngredient>> GetAccountIngredientsByIngredientId(Guid ingredientId);
        Task CreateAccountIngredient(AccountIngredient accountIngredient);
        Task UpdateAccountIngredient(AccountIngredient accountIngredient);
        Task DeleteAccountIngredient(AccountIngredient accountIngredient);
    }

    public class AccountIngredientService : IAccountIngredientService
    {
        private readonly IAccountIngredientRepo _repository;
        private readonly ITransactionManager _transactionManager;

        public AccountIngredientService(IAccountIngredientRepo repo, ITransactionManager transactionManager)
        {
            _repository = repo;
            _transactionManager = transactionManager;
        }

        public async Task<AccountIngredient?> GetAccountIngredientByIds(Guid accountId, Guid ingredientId)
        {
            return await _repository.GetAccountIngredient(accountId, ingredientId);
        }

        public async Task<List<AccountIngredient>> GetAccountIngredients()
        {
            return await _repository.GetAccountIngredients();
        }

        public async Task<List<AccountIngredient>> GetAccountIngredientsByAccountId(Guid accountId)
        {
            return await _repository.GetAccountIngredientsByAccountId(accountId);
        }

        public async Task<List<AccountIngredient>> GetAccountIngredientsByIngredientId(Guid ingredientId)
        {
            return await _repository.GetAccountIngredientsByIngredientId(ingredientId);
        }

        public async Task CreateAccountIngredient(AccountIngredient accountIngredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.CreateAccountIngredient(accountIngredient);
            });
        }

        public async Task UpdateAccountIngredient(AccountIngredient accountIngredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.UpdateAccountIngredient(accountIngredient);
            });
        }

        public async Task DeleteAccountIngredient(AccountIngredient accountIngredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.DeleteAccountIngredient(accountIngredient);
            });
        }
    }
}

