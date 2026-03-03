using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Data
{
    public interface IIngredientRepo
    {
        Task<Ingredient?> GetIngredient(string name);
        Task<List<Ingredient>> GetIngredients();
        Task CreateIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
        Task DeleteIngredient(Ingredient ingredient);
        Task SaveChangeAsync();
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
            await _context.Ingredients.AddAsync(ingredient);
        }

        public async Task<Ingredient?> GetIngredient(string name)
        {
            return await _context.Ingredients.Where(i => i.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<List<Ingredient>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
