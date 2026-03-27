using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountIngredientController : ControllerBase
    {
        private readonly ILogger<AccountIngredientController> _logger;
        private readonly IAccountIngredientService _accountIngredientService;
        private readonly IAccountService _accountService;
        private readonly IIngredientService _ingredientService;

        public AccountIngredientController(ILogger<AccountIngredientController> logger, IAccountIngredientService accountIngredientService, IAccountService accountService, IIngredientService ingredientService)
        {
            _logger = logger;
            _accountIngredientService = accountIngredientService;
            _accountService = accountService;
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<List<AccountIngredientResponse>> GetAccountIngredients(Guid? accountId, Guid? ingredientId)
        {
            if (accountId.HasValue && ingredientId.HasValue)
            {
                var single = await _accountIngredientService.GetAccountIngredientByIds(accountId.Value, ingredientId.Value);
                return single != null ? new List<AccountIngredientResponse> { new AccountIngredientResponse { AccountId = single.AccountId, IngredientId = single.IngredientId, Amount = single.Amount } } : new List<AccountIngredientResponse>();
            }

            if (accountId.HasValue)
            {
                var ingredients = await _accountIngredientService.GetAccountIngredientsByAccountId(accountId.Value);
                return ingredients.Select(ai => new AccountIngredientResponse { AccountId = ai.AccountId, IngredientId = ai.IngredientId, Amount = ai.Amount }).ToList();
            }

            if (ingredientId.HasValue)
            {
                var ingredients = await _accountIngredientService.GetAccountIngredientsByIngredientId(ingredientId.Value);
                return ingredients.Select(ai => new AccountIngredientResponse { AccountId = ai.AccountId, IngredientId = ai.IngredientId, Amount = ai.Amount }).ToList();
            }

            var accountIngredients = await _accountIngredientService.GetAccountIngredients();
            return accountIngredients.Select(ai => new AccountIngredientResponse { AccountId = ai.AccountId, IngredientId = ai.IngredientId, Amount = ai.Amount }).ToList();
        }

        [HttpGet("{accountId:guid}/{ingredientId:guid}")]
        public async Task<ActionResult<AccountIngredient>> GetAccountIngredient(Guid accountId, Guid ingredientId)
        {
            var accountIngredient = await _accountIngredientService.GetAccountIngredientByIds(accountId, ingredientId);
            if (accountIngredient == null)
            {
                return NotFound();
            }

            return Ok(new AccountIngredientResponse { AccountId = accountIngredient.AccountId, IngredientId = accountIngredient.IngredientId, Amount = accountIngredient.Amount });
        }

        [HttpPost]
        public async Task<ActionResult<AccountIngredient>> CreateAccountIngredient(AccountIngredientRequest accountIngredientDto)
        {
            if (accountIngredientDto == null)
            {
                return BadRequest();
            }

            if (accountIngredientDto.AccountId == Guid.Empty || accountIngredientDto.IngredientId == Guid.Empty)
            {
                return BadRequest("AccountId and IngredientId are required.");
            }

            var account = await _accountService.GetAccountById(accountIngredientDto.AccountId);
            if (account == null)
            {
                return BadRequest("Account not found.");
            }

            var ingredient = await _ingredientService.GetIngredientById(accountIngredientDto.IngredientId);
            if (ingredient == null)
            {
                return BadRequest("Ingredient not found.");
            }

            var accountIngredient = new AccountIngredient
            {
                AccountId = accountIngredientDto.AccountId,
                IngredientId = accountIngredientDto.IngredientId,
                Amount = accountIngredientDto.Amount
            };

            await _accountIngredientService.CreateAccountIngredient(accountIngredient);

            return CreatedAtAction(
                nameof(GetAccountIngredient),
                new { accountId = accountIngredient.AccountId, ingredientId = accountIngredient.IngredientId },
                new AccountIngredientResponse { AccountId = accountIngredient.AccountId, IngredientId = accountIngredient.IngredientId, Amount = accountIngredient.Amount });
        }

        [HttpPut("{accountId:guid}/{ingredientId:guid}")]
        public async Task<IActionResult> UpdateAccountIngredient(Guid accountId, Guid ingredientId, AccountIngredientRequest accountIngredientDto)
        {
            if (accountIngredientDto == null)
            {
                return BadRequest();
            }

            if (accountIngredientDto.AccountId != Guid.Empty && accountIngredientDto.AccountId != accountId)
            {
                return BadRequest("AccountId in body does not match route id.");
            }

            if (accountIngredientDto.IngredientId != Guid.Empty && accountIngredientDto.IngredientId != ingredientId)
            {
                return BadRequest("IngredientId in body does not match route id.");
            }

            var existing = await _accountIngredientService.GetAccountIngredientByIds(accountId, ingredientId);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Amount = accountIngredientDto.Amount;
            await _accountIngredientService.UpdateAccountIngredient(existing);
            return NoContent();
        }

        [HttpDelete("{accountId:guid}/{ingredientId:guid}")]
        public async Task<IActionResult> DeleteAccountIngredient(Guid accountId, Guid ingredientId)
        {
            var existing = await _accountIngredientService.GetAccountIngredientByIds(accountId, ingredientId);
            if (existing == null)
            {
                return NotFound();
            }

            await _accountIngredientService.DeleteAccountIngredient(existing);
            return NoContent();
        }
    }
}

