using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly ILogger<IngredientController> _logger;
        private readonly IIngredientService _ingredientService;

        public IngredientController(ILogger<IngredientController> logger, IIngredientService ingredientService)
        {
            _logger = logger;
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<List<Ingredient>> GetIngredients(string? name)
        {
            var ingredients = new List<Ingredient>();
            if (!string.IsNullOrEmpty(name))
            {
                var matchingIngredient = await _ingredientService.GetIngredient(name);
                if (matchingIngredient != null)
                {
                    ingredients.Add(matchingIngredient);
                }
            }
            else
            {
                ingredients = await _ingredientService.GetIngredients();
            }

            return ingredients;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(Guid id)
        {
            var ingredient = await _ingredientService.GetIngredientById(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        [HttpPost]
        public async Task<ActionResult<Ingredient>> CreateIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest();
            }

            await _ingredientService.CreateIngredient(ingredient);

            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, ingredient);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateIngredient(Guid id, Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest();
            }

            if (ingredient.Id != Guid.Empty && ingredient.Id != id)
            {
                return BadRequest("Ingredient id in body does not match route id.");
            }

            var existing = await _ingredientService.GetIngredientById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = ingredient.Name;
            existing.Description = ingredient.Description;
            existing.LastUpdatedOn = DateTime.UtcNow;

            await _ingredientService.UpdateIngredient(existing);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var existing = await _ingredientService.GetIngredientById(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _ingredientService.DeleteIngredient(existing);

            return NoContent();
        }
    }
}
