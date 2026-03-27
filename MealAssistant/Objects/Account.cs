using System.Text.Json.Serialization;

namespace MealAssistant.Objects
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime LastLoggedIn { get; set; }

        [JsonIgnore]
        public List<AccountIngredient> AccountIngredients { get; set; } = new List<AccountIngredient>();
    }
}
