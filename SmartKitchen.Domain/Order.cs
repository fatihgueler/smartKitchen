namespace SmartKitchen.Domain;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Neu";
    public string Notes { get; set; } = "";
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}
