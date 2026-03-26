using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Data
{
    public interface IAccountIngredientRepo
    {
        Task<AccountIngredient?> GetAccountIngredient(Guid accountId, Guid ingredientId);
        Task<List<AccountIngredient>> GetAccountIngredients();
        Task<List<AccountIngredient>> GetAccountIngredientsByAccountId(Guid accountId);
        Task<List<AccountIngredient>> GetAccountIngredientsByIngredientId(Guid ingredientId);
        Task CreateAccountIngredient(AccountIngredient accountIngredient);
        Task UpdateAccountIngredient(AccountIngredient accountIngredient);
        Task DeleteAccountIngredient(AccountIngredient accountIngredient);
    }

    public class AccountIngredientRepo : IAccountIngredientRepo
    {
        private readonly AppDbContext _context;

        public AccountIngredientRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountIngredient?> GetAccountIngredient(Guid accountId, Guid ingredientId)
        {
            return await _context.AccountIngredients
                .FirstOrDefaultAsync(ai => ai.AccountId == accountId && ai.IngredientId == ingredientId);
        }

        public async Task<List<AccountIngredient>> GetAccountIngredients()
        {
            return await _context.AccountIngredients.ToListAsync();
        }

        public async Task<List<AccountIngredient>> GetAccountIngredientsByAccountId(Guid accountId)
        {
            return await _context.AccountIngredients
                .Where(ai => ai.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<List<AccountIngredient>> GetAccountIngredientsByIngredientId(Guid ingredientId)
        {
            return await _context.AccountIngredients
                .Where(ai => ai.IngredientId == ingredientId)
                .ToListAsync();
        }

        public async Task CreateAccountIngredient(AccountIngredient accountIngredient)
        {
            await _context.AccountIngredients.AddAsync(accountIngredient);
            // no SaveChanges here (handled by TransactionManager in service)
        }

        public async Task UpdateAccountIngredient(AccountIngredient accountIngredient)
        {
            _context.AccountIngredients.Update(accountIngredient);
            // no SaveChanges here (handled by TransactionManager in service)
            await Task.CompletedTask;
        }

        public async Task DeleteAccountIngredient(AccountIngredient accountIngredient)
        {
            _context.AccountIngredients.Remove(accountIngredient);
            // no SaveChanges here (handled by TransactionManager in service)
            await Task.CompletedTask;
        }
    }
}

