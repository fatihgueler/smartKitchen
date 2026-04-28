namespace SmartKitchen.Domain;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Instructions { get; set; } = "";
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; } = 4;
    public string Category { get; set; } = "Hauptgericht";
    public string Difficulty { get; set; } = "Mittel";
    public string ImageUrl { get; set; } = "";
    public decimal EstimatedCost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<RecipeIngredient> Ingredients { get; set; } = new();
}
