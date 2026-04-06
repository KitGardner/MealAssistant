using MealAssistant.Objects;
using Microsoft.EntityFrameworkCore;

namespace MealAssistant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountIngredient> AccountIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountIngredient>()
                .HasKey(ai => new { ai.AccountId, ai.IngredientId });

            modelBuilder.Entity<AccountIngredient>()
                .HasOne(a => a.Account)
                .WithMany(a => a.AccountIngredients)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccountIngredient>()
                .HasOne(a => a.Ingredient)
                .WithMany(i => i.AccountIngredients)
                .HasForeignKey(a => a.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
