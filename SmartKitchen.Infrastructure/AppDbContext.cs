using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;

namespace SmartKitchen.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.RecipeId);

        modelBuilder.Entity<RecipeIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.RecipeIngredients)
            .HasForeignKey(ri => ri.IngredientId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Recipe)
            .WithMany()
            .HasForeignKey(oi => oi.RecipeId);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Ingredient)
            .WithMany()
            .HasForeignKey(ii => ii.IngredientId);

        modelBuilder.Entity<MealPlan>()
            .HasOne(mp => mp.Recipe)
            .WithMany()
            .HasForeignKey(mp => mp.RecipeId);

        modelBuilder.Entity<ShoppingListItem>()
            .HasOne(si => si.Ingredient)
            .WithMany()
            .HasForeignKey(si => si.IngredientId);

        // Seed data
        var ingredients = new[]
        {
            new Ingredient { Id = 1, Name = "Spaghetti", Unit = "g", Category = "Nudeln", PricePerUnit = 0.003m },
            new Ingredient { Id = 2, Name = "Hackfleisch", Unit = "g", Category = "Fleisch", PricePerUnit = 0.009m },
            new Ingredient { Id = 3, Name = "Tomatensoße", Unit = "ml", Category = "Soßen", PricePerUnit = 0.004m },
            new Ingredient { Id = 4, Name = "Zwiebel", Unit = "Stück", Category = "Gemüse", PricePerUnit = 0.30m },
            new Ingredient { Id = 5, Name = "Knoblauch", Unit = "Zehe", Category = "Gemüse", PricePerUnit = 0.15m },
            new Ingredient { Id = 6, Name = "Olivenöl", Unit = "ml", Category = "Öle", PricePerUnit = 0.01m },
            new Ingredient { Id = 7, Name = "Parmesan", Unit = "g", Category = "Käse", PricePerUnit = 0.02m },
            new Ingredient { Id = 8, Name = "Hähnchenbrust", Unit = "g", Category = "Fleisch", PricePerUnit = 0.012m },
            new Ingredient { Id = 9, Name = "Reis", Unit = "g", Category = "Getreide", PricePerUnit = 0.002m },
            new Ingredient { Id = 10, Name = "Brokkoli", Unit = "g", Category = "Gemüse", PricePerUnit = 0.005m },
            new Ingredient { Id = 11, Name = "Sahne", Unit = "ml", Category = "Milchprodukte", PricePerUnit = 0.003m },
            new Ingredient { Id = 12, Name = "Butter", Unit = "g", Category = "Milchprodukte", PricePerUnit = 0.008m },
            new Ingredient { Id = 13, Name = "Mehl", Unit = "g", Category = "Backen", PricePerUnit = 0.001m },
            new Ingredient { Id = 14, Name = "Eier", Unit = "Stück", Category = "Milchprodukte", PricePerUnit = 0.25m },
            new Ingredient { Id = 15, Name = "Salz", Unit = "g", Category = "Gewürze", PricePerUnit = 0.001m },
            new Ingredient { Id = 16, Name = "Pfeffer", Unit = "g", Category = "Gewürze", PricePerUnit = 0.05m },
            new Ingredient { Id = 17, Name = "Paprika", Unit = "Stück", Category = "Gemüse", PricePerUnit = 0.80m },
            new Ingredient { Id = 18, Name = "Champignons", Unit = "g", Category = "Gemüse", PricePerUnit = 0.006m },
            new Ingredient { Id = 19, Name = "Kartoffeln", Unit = "g", Category = "Gemüse", PricePerUnit = 0.002m },
            new Ingredient { Id = 20, Name = "Lachs", Unit = "g", Category = "Fisch", PricePerUnit = 0.025m },
        };
        modelBuilder.Entity<Ingredient>().HasData(ingredients);

        var recipes = new[]
        {
            new { Id = 1, Name = "Spaghetti Bolognese", Description = "Klassische italienische Pasta mit reichhaltiger Fleischsoße", Instructions = "1. Zwiebel und Knoblauch fein hacken und in Olivenöl anbraten.\n2. Hackfleisch hinzufügen und krümelig braten.\n3. Tomatensoße hinzugeben und 20 Minuten köcheln lassen.\n4. Spaghetti nach Packungsanleitung kochen.\n5. Soße über die Pasta geben und mit Parmesan servieren.", PrepTimeMinutes = 15, CookTimeMinutes = 30, Servings = 4, Category = "Hauptgericht", Difficulty = "Einfach", ImageUrl = "", EstimatedCost = 8.50m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, Name = "Hähnchen-Reis-Bowl", Description = "Gesunde Bowl mit zartem Hähnchen, Reis und frischem Gemüse", Instructions = "1. Reis nach Packungsanleitung kochen.\n2. Hähnchenbrust in Streifen schneiden und würzen.\n3. Hähnchen in der Pfanne goldbraun braten.\n4. Brokkoli dampfgaren.\n5. Alles in einer Bowl anrichten.", PrepTimeMinutes = 10, CookTimeMinutes = 25, Servings = 2, Category = "Hauptgericht", Difficulty = "Einfach", ImageUrl = "", EstimatedCost = 6.00m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, Name = "Pilzrisotto", Description = "Cremiges Risotto mit frischen Champignons und Parmesan", Instructions = "1. Champignons in Scheiben schneiden.\n2. Zwiebel fein hacken und in Butter anschwitzen.\n3. Reis hinzufügen und glasig rühren.\n4. Nach und nach warme Brühe hinzufügen.\n5. Champignons und Parmesan unterrühren.", PrepTimeMinutes = 10, CookTimeMinutes = 35, Servings = 4, Category = "Hauptgericht", Difficulty = "Mittel", ImageUrl = "", EstimatedCost = 7.00m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 4, Name = "Lachs mit Kartoffeln", Description = "Gebratener Lachs auf einem Bett aus Kartoffelpüree", Instructions = "1. Kartoffeln schälen und kochen.\n2. Lachs würzen und in der Pfanne braten.\n3. Kartoffeln stampfen mit Butter und Sahne.\n4. Lachs auf dem Püree anrichten.\n5. Mit frischen Kräutern garnieren.", PrepTimeMinutes = 20, CookTimeMinutes = 25, Servings = 2, Category = "Hauptgericht", Difficulty = "Mittel", ImageUrl = "", EstimatedCost = 12.00m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 5, Name = "Gemüsepfanne", Description = "Bunte Gemüsepfanne mit Paprika, Champignons und Brokkoli", Instructions = "1. Alles Gemüse waschen und schneiden.\n2. Olivenöl in einer großen Pfanne erhitzen.\n3. Gemüse nach Garzeit sortiert hinzufügen.\n4. Mit Salz, Pfeffer und Gewürzen abschmecken.\n5. Optional mit Reis servieren.", PrepTimeMinutes = 15, CookTimeMinutes = 15, Servings = 4, Category = "Hauptgericht", Difficulty = "Einfach", ImageUrl = "", EstimatedCost = 5.00m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 6, Name = "Pfannkuchen", Description = "Fluffige Pfannkuchen – perfekt zum Frühstück oder als Dessert", Instructions = "1. Mehl, Eier, Milch und eine Prise Salz verrühren.\n2. Butter in einer Pfanne schmelzen.\n3. Teig portionsweise in die Pfanne geben.\n4. Von beiden Seiten goldbraun backen.\n5. Mit Zucker, Zimt oder Früchten servieren.", PrepTimeMinutes = 10, CookTimeMinutes = 15, Servings = 4, Category = "Frühstück", Difficulty = "Einfach", ImageUrl = "", EstimatedCost = 3.00m, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
        };
        modelBuilder.Entity<Recipe>().HasData(recipes);

        var recipeIngredients = new[]
        {
            // Spaghetti Bolognese
            new { Id = 1, RecipeId = 1, IngredientId = 1, Amount = 500m },
            new { Id = 2, RecipeId = 1, IngredientId = 2, Amount = 400m },
            new { Id = 3, RecipeId = 1, IngredientId = 3, Amount = 500m },
            new { Id = 4, RecipeId = 1, IngredientId = 4, Amount = 2m },
            new { Id = 5, RecipeId = 1, IngredientId = 5, Amount = 3m },
            new { Id = 6, RecipeId = 1, IngredientId = 6, Amount = 30m },
            new { Id = 7, RecipeId = 1, IngredientId = 7, Amount = 50m },
            // Hähnchen-Reis-Bowl
            new { Id = 8, RecipeId = 2, IngredientId = 8, Amount = 300m },
            new { Id = 9, RecipeId = 2, IngredientId = 9, Amount = 200m },
            new { Id = 10, RecipeId = 2, IngredientId = 10, Amount = 200m },
            new { Id = 11, RecipeId = 2, IngredientId = 6, Amount = 20m },
            // Pilzrisotto
            new { Id = 12, RecipeId = 3, IngredientId = 9, Amount = 300m },
            new { Id = 13, RecipeId = 3, IngredientId = 18, Amount = 250m },
            new { Id = 14, RecipeId = 3, IngredientId = 4, Amount = 1m },
            new { Id = 15, RecipeId = 3, IngredientId = 12, Amount = 40m },
            new { Id = 16, RecipeId = 3, IngredientId = 7, Amount = 60m },
            // Lachs mit Kartoffeln
            new { Id = 17, RecipeId = 4, IngredientId = 20, Amount = 400m },
            new { Id = 18, RecipeId = 4, IngredientId = 19, Amount = 600m },
            new { Id = 19, RecipeId = 4, IngredientId = 12, Amount = 30m },
            new { Id = 20, RecipeId = 4, IngredientId = 11, Amount = 100m },
            // Gemüsepfanne
            new { Id = 21, RecipeId = 5, IngredientId = 17, Amount = 2m },
            new { Id = 22, RecipeId = 5, IngredientId = 18, Amount = 200m },
            new { Id = 23, RecipeId = 5, IngredientId = 10, Amount = 200m },
            new { Id = 24, RecipeId = 5, IngredientId = 4, Amount = 1m },
            new { Id = 25, RecipeId = 5, IngredientId = 6, Amount = 30m },
            // Pfannkuchen
            new { Id = 26, RecipeId = 6, IngredientId = 13, Amount = 250m },
            new { Id = 27, RecipeId = 6, IngredientId = 14, Amount = 3m },
            new { Id = 28, RecipeId = 6, IngredientId = 12, Amount = 30m },
            new { Id = 29, RecipeId = 6, IngredientId = 15, Amount = 5m },
        };
        modelBuilder.Entity<RecipeIngredient>().HasData(recipeIngredients);

        // Seed inventory
        var inventoryItems = new[]
        {
            new { Id = 1, IngredientId = 1, CurrentStock = 1000m, MinimumStock = 500m, ExpiryDate = (DateTime?)new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 2, IngredientId = 6, CurrentStock = 500m, MinimumStock = 200m, ExpiryDate = (DateTime?)new DateTime(2026, 10, 15, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 3, IngredientId = 9, CurrentStock = 2000m, MinimumStock = 500m, ExpiryDate = (DateTime?)new DateTime(2027, 3, 1, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 4, IngredientId = 15, CurrentStock = 500m, MinimumStock = 100m, ExpiryDate = (DateTime?)null, LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 5, IngredientId = 16, CurrentStock = 100m, MinimumStock = 50m, ExpiryDate = (DateTime?)null, LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 6, IngredientId = 13, CurrentStock = 2000m, MinimumStock = 500m, ExpiryDate = (DateTime?)new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 7, IngredientId = 12, CurrentStock = 200m, MinimumStock = 100m, ExpiryDate = (DateTime?)new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = 8, IngredientId = 14, CurrentStock = 12m, MinimumStock = 6m, ExpiryDate = (DateTime?)new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc), LastUpdated = new DateTime(2026, 4, 27, 0, 0, 0, DateTimeKind.Utc) },
        };
        modelBuilder.Entity<InventoryItem>().HasData(inventoryItems);
    }
}
