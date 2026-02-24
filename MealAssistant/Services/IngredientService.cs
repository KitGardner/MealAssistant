using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Services
{
    public interface IIngredientService
    {
        Ingredient GetIngredient(string name);
        List<Ingredient> GetIngredients();
        Guid CreateIngredient(Ingredient ingredient);
        Guid UpdateIngredient(Ingredient ingredient);
        bool DeleteIngredient(Guid id);
    }

    public class IngredientService : IIngredientService
    {
        private IIngredientRepo? _ingredientRepo;
        public IIngredientRepo IngredientRepo
        {
            get => _ingredientRepo ??= new IngredientRepo();
            set => _ingredientRepo = value;
        }

        public Guid CreateIngredient(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Ingredient GetIngredient(string name)
        {
            return IngredientRepo.GetIngredient(name);
        }

        public List<Ingredient> GetIngredients()
        {
            return IngredientRepo.GetIngredients();
        }

        public Guid UpdateIngredient(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public bool DeleteIngredient(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
