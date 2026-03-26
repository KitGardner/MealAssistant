using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<List<Account>> GetAccounts(string? username)
        {
            var accounts = new List<Account>();

            if (!string.IsNullOrEmpty(username))
            {
                var matchingAccount = await _accountService.GetAccount(username);
                if (matchingAccount != null)
                {
                    accounts.Add(matchingAccount);
                }
            }
            else
            {
                accounts = await _accountService.GetAccounts();
            }

            return accounts;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            if (account == null)
            {
                return BadRequest();
            }

            await _accountService.CreateAccount(account);

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid id, Account account)
        {
            if (account == null)
            {
                return BadRequest();
            }

            if (account.Id != Guid.Empty && account.Id != id)
            {
                return BadRequest("Account id in body does not match route id.");
            }

            var existing = await _accountService.GetAccountById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.FirstName = account.FirstName;
            existing.LastName = account.LastName;
            existing.Email = account.Email;
            existing.Username = account.Username;
            existing.Password = account.Password;
            if (account.LastLoggedIn != default)
            {
                existing.LastLoggedIn = account.LastLoggedIn;
            }

            await _accountService.UpdateAccount(existing);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var existing = await _accountService.GetAccountById(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _accountService.DeleteAccount(existing);
            return NoContent();
        }
    }
}

