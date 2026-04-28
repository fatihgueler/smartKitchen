namespace SmartKitchen.Domain;

public class ShoppingListItem
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public decimal Amount { get; set; }
    public bool IsChecked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
