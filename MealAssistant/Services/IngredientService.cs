using MealAssistant.Data;
using MealAssistant.Objects;

namespace MealAssistant.Services
{
    public interface IIngredientService
    {
        Task<Ingredient?> GetIngredient(string name);
        Task<List<Ingredient>> GetIngredients();
        Task CreateIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredient(Ingredient ingredient);
    }

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepo _repository;

        public IngredientService(IIngredientRepo repo)
        {
            _repository = repo;
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            await _repository.CreateIngredient(ingredient);
        }

        public async Task<Ingredient?> GetIngredient(string name)
        {
            return await _repository.GetIngredient(name);
        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _repository.GetIngredients();
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            await _repository.UpdateIngredient(ingredient);
        }

        public async Task DeleteIngredient(Ingredient ingredient)
        {
            await _repository.DeleteIngredient(ingredient);
        }
    }
}
