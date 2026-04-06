public class AccountIngredientRequest
{
    public Guid AccountId { get; set; }
    public Guid IngredientId { get; set; }
    public int Amount { get; set; }
}