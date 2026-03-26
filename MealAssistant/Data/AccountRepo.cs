using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Data
{
    public interface IAccountRepo
    {
        Task<Account?> GetAccount(string username);
        Task<Account?> GetAccountById(Guid id);
        Task<List<Account>> GetAccounts();
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(Account account);
    }

    public class AccountRepo : IAccountRepo
    {
        private readonly AppDbContext _context;

        public AccountRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAccount(Account account)
        {
            if (account.Id == Guid.Empty)
            {
                account.Id = Guid.NewGuid();
            }

            var now = DateTime.UtcNow;
            if (account.CreatedOn == default)
            {
                account.CreatedOn = now;
            }

            if (account.LastLoggedIn == default)
            {
                account.LastLoggedIn = now;
            }

            await _context.Accounts.AddAsync(account);
            // no SaveChanges here (handled by TransactionManager in service)
        }

        public async Task<Account?> GetAccount(string username)
        {
            return await _context.Accounts
                .Where(a => a.Username.ToLower() == username.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAccountById(Guid id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            // no SaveChanges here (handled by TransactionManager in service)
            await Task.CompletedTask;
        }

        public async Task DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
            // no SaveChanges here (handled by TransactionManager in service)
            await Task.CompletedTask;
        }
    }
}

