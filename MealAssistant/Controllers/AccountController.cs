using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<List<AccountResponse>>> GetAccounts(string? username)
        {
            var accounts = new List<Account>();

            if (!string.IsNullOrEmpty(username))
            {
                var matchingAccount = await _accountService.GetAccount(username);
                if (matchingAccount != null)
                {
                    accounts.Add(matchingAccount);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                accounts = await _accountService.GetAccounts();
            }

            return Ok(accounts.Select(a => new AccountResponse
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                Username = a.Username
            }).ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountResponse>> GetAccount(Guid id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(new AccountResponse
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Username = account.Username
            });
        }

        [HttpPost]
        public async Task<ActionResult<AccountResponse>> CreateAccount(AccountRequest accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest();
            }

            var account = new Account
            {
                FirstName = accountDto.FirstName ?? string.Empty,
                LastName = accountDto.LastName ?? string.Empty,
                Email = accountDto.Email ?? string.Empty,
                Username = accountDto.Username ?? string.Empty,
                Password = accountDto.Password ?? string.Empty,
            };

            await _accountService.CreateAccount(account);

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, new AccountResponse
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Username = account.Username
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid id, AccountRequest accountDto)
        {
            if (accountDto == null)
            {
                return BadRequest();
            }

            if (accountDto.Id != Guid.Empty && accountDto.Id != id)
            {
                return BadRequest("Account id in body does not match route id.");
            }

            var existing = await _accountService.GetAccountById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.FirstName = accountDto.FirstName ?? string.Empty;
            existing.LastName = accountDto.LastName ?? string.Empty;
            existing.Email = accountDto.Email ?? string.Empty;
            existing.Username = accountDto.Username ?? string.Empty;
            existing.Password = accountDto.Password ?? string.Empty;

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

