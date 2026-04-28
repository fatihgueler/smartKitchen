namespace SmartKitchen.Domain;

public class MealPlan
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string MealType { get; set; } = "Mittagessen";
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int Servings { get; set; } = 4;
    public string Notes { get; set; } = "";
}
