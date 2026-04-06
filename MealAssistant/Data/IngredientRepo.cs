using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;
namespace MealAssistant.Data
{
    public interface IIngredientRepo
    {
        Task<Ingredient?> GetIngredient(string name);
        Task<Ingredient?> GetIngredientById(Guid id);
        Task<List<Ingredient>> GetIngredients();
        Task CreateIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredient(Ingredient ingredient);
    }
    public class IngredientRepo : IIngredientRepo
    {
        private readonly AppDbContext _context;
        public IngredientRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateIngredient(Ingredient ingredient)
        {
            if (ingredient.Id == Guid.Empty)
            {
                ingredient.Id = Guid.NewGuid();
            }
            var now = DateTime.UtcNow;
            if (ingredient.CreatedOn == default)
            {
                ingredient.CreatedOn = now;
            }
            ingredient.LastUpdatedOn = now;
            await _context.Ingredients.AddAsync(ingredient);
            // no SaveChanges here
        }
        public async Task<Ingredient?> GetIngredient(string name)
        {
            return await _context.Ingredients
                .Where(i => i.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }
        public async Task<Ingredient?> GetIngredientById(Guid id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }
        public async Task UpdateIngredient(Ingredient ingredient)
        {
            ingredient.LastUpdatedOn = DateTime.UtcNow;
            _context.Ingredients.Update(ingredient);
            // no SaveChanges here
            await Task.CompletedTask;
        }
        public async Task DeleteIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
            // no SaveChanges here
            await Task.CompletedTask;
        }
    }
}