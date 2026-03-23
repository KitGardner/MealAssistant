namespace MealAssistant.Objects
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn {  get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public List<AccountIngredient> AccountIngredients { get; set; } = new List<AccountIngredient>();
    }
}
