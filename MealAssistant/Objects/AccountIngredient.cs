namespace MealAssistant.Objects
{
    public class AccountIngredient
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
        public int Amount { get; set; }
    }
}
