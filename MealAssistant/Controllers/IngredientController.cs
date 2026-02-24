using MealAssistant.Objects;
using MealAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace MealAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientController : ControllerBase
    {
        private IIngredientService? _ingredientService;
        public IIngredientService IngredientService
        {
            get => _ingredientService ??= new IngredientService();
            set => _ingredientService = value;
        }

        private readonly ILogger<IngredientController> _logger;

        public IngredientController(ILogger<IngredientController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Ingredient> GetIngredients()
        {
            return IngredientService.GetIngredients();
        }
    }
}
