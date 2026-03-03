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

        [HttpPost]
        public async Task<IActionResult> CreateIngredient(Ingredient ingredient)
        {
            await _ingredientService.CreateIngredient(ingredient);

            return Ok();
        }
    }
}
