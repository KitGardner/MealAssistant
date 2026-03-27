using System.Text.Json.Serialization;

namespace MealAssistant.Objects
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedOn {  get; set; }
        public DateTime LastUpdatedOn { get; set; }
        [JsonIgnore]
        public List<AccountIngredient> AccountIngredients { get; set; } = new List<AccountIngredient>();
    }
}
