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
        public async Task<List<IngredientResponse>> GetIngredients(string? name)
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

            return ingredients.Select(i => new IngredientResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                LastUpdatedOn = i.LastUpdatedOn
            }).ToList();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IngredientResponse>> GetIngredient(Guid id)
        {
            var ingredient = await _ingredientService.GetIngredientById(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(new IngredientResponse
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Description = ingredient.Description,
                LastUpdatedOn = ingredient.LastUpdatedOn
            });
        }

        [HttpPost]
        public async Task<ActionResult<IngredientResponse>> CreateIngredient(IngredientRequest ingredientDto)
        {
            if (ingredientDto == null)
            {
                return BadRequest();
            }

            var ingredient = new Ingredient
            {
                Name = ingredientDto.Name ?? string.Empty,
                Description = ingredientDto.Description ?? string.Empty,
                LastUpdatedOn = ingredientDto.LastUpdatedOn
            };

            await _ingredientService.CreateIngredient(ingredient);

            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, new IngredientResponse
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Description = ingredient.Description,
                LastUpdatedOn = ingredient.LastUpdatedOn
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateIngredient(Guid id, IngredientRequest ingredientDto)
        {
            if (ingredientDto == null)
            {
                return BadRequest();
            }

            if (ingredientDto.Id != Guid.Empty && ingredientDto.Id != id)
            {
                return BadRequest("Ingredient id in body does not match route id.");
            }

            var existing = await _ingredientService.GetIngredientById(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = ingredientDto.Name ?? string.Empty;
            existing.Description = ingredientDto.Description ?? string.Empty;
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
