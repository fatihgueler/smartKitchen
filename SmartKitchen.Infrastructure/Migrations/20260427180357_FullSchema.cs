using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartKitchen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FullSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    PrepTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    CookTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Servings = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinimumStock = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListItems_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MealType = table.Column<string>(type: "TEXT", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Servings = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IngredientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Category", "Name", "PricePerUnit", "Unit" },
                values: new object[,]
                {
                    { 1, "Nudeln", "Spaghetti", 0.003m, "g" },
                    { 2, "Fleisch", "Hackfleisch", 0.009m, "g" },
                    { 3, "Soßen", "Tomatensoße", 0.004m, "ml" },
                    { 4, "Gemüse", "Zwiebel", 0.30m, "Stück" },
                    { 5, "Gemüse", "Knoblauch", 0.15m, "Zehe" },
                    { 6, "Öle", "Olivenöl", 0.01m, "ml" },
                    { 7, "Käse", "Parmesan", 0.02m, "g" },
                    { 8, "Fleisch", "Hähnchenbrust", 0.012m, "g" },
                    { 9, "Getreide", "Reis", 0.002m, "g" },
                    { 10, "Gemüse", "Brokkoli", 0.005m, "g" },
                    { 11, "Milchprodukte", "Sahne", 0.003m, "ml" },
                    { 12, "Milchprodukte", "Butter", 0.008m, "g" },
                    { 13, "Backen", "Mehl", 0.001m, "g" },
                    { 14, "Milchprodukte", "Eier", 0.25m, "Stück" },
                    { 15, "Gewürze", "Salz", 0.001m, "g" },
                    { 16, "Gewürze", "Pfeffer", 0.05m, "g" },
                    { 17, "Gemüse", "Paprika", 0.80m, "Stück" },
                    { 18, "Gemüse", "Champignons", 0.006m, "g" },
                    { 19, "Gemüse", "Kartoffeln", 0.002m, "g" },
                    { 20, "Fisch", "Lachs", 0.025m, "g" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Category", "CookTimeMinutes", "CreatedAt", "Description", "Difficulty", "EstimatedCost", "ImageUrl", "Instructions", "Name", "PrepTimeMinutes", "Servings" },
                values: new object[,]
                {
                    { 1, "Hauptgericht", 30, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Klassische italienische Pasta mit reichhaltiger Fleischsoße", "Einfach", 8.50m, "", "1. Zwiebel und Knoblauch fein hacken und in Olivenöl anbraten.\n2. Hackfleisch hinzufügen und krümelig braten.\n3. Tomatensoße hinzugeben und 20 Minuten köcheln lassen.\n4. Spaghetti nach Packungsanleitung kochen.\n5. Soße über die Pasta geben und mit Parmesan servieren.", "Spaghetti Bolognese", 15, 4 },
                    { 2, "Hauptgericht", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gesunde Bowl mit zartem Hähnchen, Reis und frischem Gemüse", "Einfach", 6.00m, "", "1. Reis nach Packungsanleitung kochen.\n2. Hähnchenbrust in Streifen schneiden und würzen.\n3. Hähnchen in der Pfanne goldbraun braten.\n4. Brokkoli dampfgaren.\n5. Alles in einer Bowl anrichten.", "Hähnchen-Reis-Bowl", 10, 2 },
                    { 3, "Hauptgericht", 35, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cremiges Risotto mit frischen Champignons und Parmesan", "Mittel", 7.00m, "", "1. Champignons in Scheiben schneiden.\n2. Zwiebel fein hacken und in Butter anschwitzen.\n3. Reis hinzufügen und glasig rühren.\n4. Nach und nach warme Brühe hinzufügen.\n5. Champignons und Parmesan unterrühren.", "Pilzrisotto", 10, 4 },
                    { 4, "Hauptgericht", 25, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gebratener Lachs auf einem Bett aus Kartoffelpüree", "Mittel", 12.00m, "", "1. Kartoffeln schälen und kochen.\n2. Lachs würzen und in der Pfanne braten.\n3. Kartoffeln stampfen mit Butter und Sahne.\n4. Lachs auf dem Püree anrichten.\n5. Mit frischen Kräutern garnieren.", "Lachs mit Kartoffeln", 20, 2 },
                    { 5, "Hauptgericht", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bunte Gemüsepfanne mit Paprika, Champignons und Brokkoli", "Einfach", 5.00m, "", "1. Alles Gemüse waschen und schneiden.\n2. Olivenöl in einer großen Pfanne erhitzen.\n3. Gemüse nach Garzeit sortiert hinzufügen.\n4. Mit Salz, Pfeffer und Gewürzen abschmecken.\n5. Optional mit Reis servieren.", "Gemüsepfanne", 15, 4 },
                    { 6, "Frühstück", 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fluffige Pfannkuchen – perfekt zum Frühstück oder als Dessert", "Einfach", 3.00m, "", "1. Mehl, Eier, Milch und eine Prise Salz verrühren.\n2. Butter in einer Pfanne schmelzen.\n3. Teig portionsweise in die Pfanne geben.\n4. Von beiden Seiten goldbraun backen.\n5. Mit Zucker, Zimt oder Früchten servieren.", "Pfannkuchen", 10, 4 }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "CurrentStock", "ExpiryDate", "IngredientId", "LastUpdated", "MinimumStock" },
                values: new object[,]
                {
                    { 1, 1000m, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 500m },
                    { 2, 500m, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 200m },
                    { 3, 2000m, new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 500m },
                    { 4, 500m, null, 15, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 100m },
                    { 5, 100m, null, 16, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 50m },
                    { 6, 2000m, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 500m },
                    { 7, 200m, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 12, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 100m },
                    { 8, 12m, new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 14, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Utc), 6m }
                });

            migrationBuilder.InsertData(
                table: "RecipeIngredients",
                columns: new[] { "Id", "Amount", "IngredientId", "RecipeId" },
                values: new object[,]
                {
                    { 1, 500m, 1, 1 },
                    { 2, 400m, 2, 1 },
                    { 3, 500m, 3, 1 },
                    { 4, 2m, 4, 1 },
                    { 5, 3m, 5, 1 },
                    { 6, 30m, 6, 1 },
                    { 7, 50m, 7, 1 },
                    { 8, 300m, 8, 2 },
                    { 9, 200m, 9, 2 },
                    { 10, 200m, 10, 2 },
                    { 11, 20m, 6, 2 },
                    { 12, 300m, 9, 3 },
                    { 13, 250m, 18, 3 },
                    { 14, 1m, 4, 3 },
                    { 15, 40m, 12, 3 },
                    { 16, 60m, 7, 3 },
                    { 17, 400m, 20, 4 },
                    { 18, 600m, 19, 4 },
                    { 19, 30m, 12, 4 },
                    { 20, 100m, 11, 4 },
                    { 21, 2m, 17, 5 },
                    { 22, 200m, 18, 5 },
                    { 23, 200m, 10, 5 },
                    { 24, 1m, 4, 5 },
                    { 25, 30m, 6, 5 },
                    { 26, 250m, 13, 6 },
                    { 27, 3m, 14, 6 },
                    { 28, 30m, 12, 6 },
                    { 29, 5m, 15, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_IngredientId",
                table: "InventoryItems",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_RecipeId",
                table: "MealPlans",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RecipeId",
                table: "OrderItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_IngredientId",
                table: "ShoppingListItems",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "RecipeIngredients");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Ingredients");
        }
    }
}
