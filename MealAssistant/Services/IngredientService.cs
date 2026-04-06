using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MealAssistant.Data;
using MealAssistant.Objects;
namespace MealAssistant.Services
{
    public interface IIngredientService
    {
        Task<Ingredient?> GetIngredient(string name);
        Task<Ingredient?> GetIngredientById(Guid id);
        Task<List<Ingredient>> GetIngredients();
        Task CreateIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredient(Ingredient ingredient);
    }
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepo _repository;
        private readonly ITransactionManager _transactionManager;

        public IngredientService(IIngredientRepo repo, ITransactionManager transactionManager)
        {
            _repository = repo;
            _transactionManager = transactionManager;
        }
        public async Task<Ingredient?> GetIngredient(string name)
        {
            return await _repository.GetIngredient(name);
        }
        public async Task<Ingredient?> GetIngredientById(Guid id)
        {
            return await _repository.GetIngredientById(id);
        }
        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _repository.GetIngredients();
        }
        public async Task CreateIngredient(Ingredient ingredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.CreateIngredient(ingredient);
            });
        }
        public async Task UpdateIngredient(Ingredient ingredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.UpdateIngredient(ingredient);
            });
        }
        public async Task DeleteIngredient(Ingredient ingredient)
        {
            await _transactionManager.ExecuteInTransactionWithSaveAsync(async () =>
            {
                await _repository.DeleteIngredient(ingredient);
            });
        }
    }
}