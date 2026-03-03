using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ingredient> Ingredients { get; set; }
    }
}
