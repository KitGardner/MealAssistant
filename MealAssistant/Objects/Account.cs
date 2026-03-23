namespace MealAssistant.Objects
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public List<AccountIngredient> AccountIngredients { get; set; } = new List<AccountIngredient>();
    }
}
