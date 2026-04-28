namespace SmartKitchen.Domain;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Unit { get; set; } = "g";
    public string Category { get; set; } = "Sonstiges";
    public decimal PricePerUnit { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
}

public class RecipeIngredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    public decimal Amount { get; set; }
}
