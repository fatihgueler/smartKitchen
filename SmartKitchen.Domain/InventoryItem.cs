namespace SmartKitchen.Domain;

public class InventoryItem
{
    public int Id { get; set; }
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public decimal CurrentStock { get; set; }
    public decimal MinimumStock { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
