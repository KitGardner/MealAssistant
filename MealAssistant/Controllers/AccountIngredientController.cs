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

        public AccountIngredientController(ILogger<AccountIngredientController> logger, IAccountIngredientService accountIngredientService)
        {
            _logger = logger;
            _accountIngredientService = accountIngredientService;
        }

        [HttpGet]
        public async Task<List<AccountIngredient>> GetAccountIngredients(Guid? accountId, Guid? ingredientId)
        {
            if (accountId.HasValue && ingredientId.HasValue)
            {
                var single = await _accountIngredientService.GetAccountIngredientByIds(accountId.Value, ingredientId.Value);
                return single != null ? new List<AccountIngredient> { single } : new List<AccountIngredient>();
            }

            if (accountId.HasValue)
            {
                return await _accountIngredientService.GetAccountIngredientsByAccountId(accountId.Value);
            }

            if (ingredientId.HasValue)
            {
                return await _accountIngredientService.GetAccountIngredientsByIngredientId(ingredientId.Value);
            }

            return await _accountIngredientService.GetAccountIngredients();
        }

        [HttpGet("{accountId:guid}/{ingredientId:guid}")]
        public async Task<ActionResult<AccountIngredient>> GetAccountIngredient(Guid accountId, Guid ingredientId)
        {
            var accountIngredient = await _accountIngredientService.GetAccountIngredientByIds(accountId, ingredientId);
            if (accountIngredient == null)
            {
                return NotFound();
            }

            return Ok(accountIngredient);
        }

        [HttpPost]
        public async Task<ActionResult<AccountIngredient>> CreateAccountIngredient(AccountIngredient accountIngredient)
        {
            if (accountIngredient == null)
            {
                return BadRequest();
            }

            if (accountIngredient.AccountId == Guid.Empty || accountIngredient.IngredientId == Guid.Empty)
            {
                return BadRequest("AccountId and IngredientId are required.");
            }

            await _accountIngredientService.CreateAccountIngredient(accountIngredient);

            return CreatedAtAction(
                nameof(GetAccountIngredient),
                new { accountId = accountIngredient.AccountId, ingredientId = accountIngredient.IngredientId },
                accountIngredient);
        }

        [HttpPut("{accountId:guid}/{ingredientId:guid}")]
        public async Task<IActionResult> UpdateAccountIngredient(Guid accountId, Guid ingredientId, AccountIngredient accountIngredient)
        {
            if (accountIngredient == null)
            {
                return BadRequest();
            }

            if (accountIngredient.AccountId != Guid.Empty && accountIngredient.AccountId != accountId)
            {
                return BadRequest("AccountId in body does not match route id.");
            }

            if (accountIngredient.IngredientId != Guid.Empty && accountIngredient.IngredientId != ingredientId)
            {
                return BadRequest("IngredientId in body does not match route id.");
            }

            var existing = await _accountIngredientService.GetAccountIngredientByIds(accountId, ingredientId);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Amount = accountIngredient.Amount;
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

