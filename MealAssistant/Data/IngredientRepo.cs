using MealAssistant.Objects;

namespace MealAssistant.Data
{
    public interface IIngredientRepo
    {
        Ingredient GetIngredient(string name);
        List<Ingredient> GetIngredients();
        Guid CreateIngredient(Ingredient ingredient);
        Guid UpdateIngredient(Ingredient ingredient);
        bool DeleteIngredient(Guid id);
    }

    public class IngredientRepo : IIngredientRepo
    {
        public Guid CreateIngredient(Ingredient ingredient)
        {
            throw new NotImplementedException();
        }

        public Ingredient GetIngredient(string name)
        {
            return new Ingredient
            {
                Name = name,
                Id = Guid.NewGuid(),
                Amount = 5
            };
        }

        public List<Ingredient> GetIngredients()
        {
            return new List<Ingredient>
            {
                new Ingredient
                {
                    Name = "Onion",
                    Id = Guid.NewGuid(),
                    Amount = 5
                },
                new Ingredient
                {
                    Name = "Carrot",
                    Id = Guid.NewGuid(),
                    Amount = 5
                },
                new Ingredient
                {
                    Name = "Beef",
                    Id = Guid.NewGuid(),
                    Amount = 5
                },
                new Ingredient
                {
                    Name = "Orange",
                    Id = Guid.NewGuid(),
                    Amount = 5
                },
            };
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
