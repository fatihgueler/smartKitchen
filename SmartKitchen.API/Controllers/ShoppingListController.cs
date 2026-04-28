using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/shoppinglist")]
public class ShoppingListController : ControllerBase
{
    private readonly AppDbContext _context;

    public ShoppingListController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.ShoppingListItems
            .Include(s => s.Ingredient)
            .OrderBy(s => s.Ingredient.Category)
            .ThenBy(s => s.Ingredient.Name)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ShoppingListItem item)
    {
        item.CreatedAt = DateTime.UtcNow;
        _context.ShoppingListItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPost("generate-from-mealplan")]
    public async Task<IActionResult> GenerateFromMealPlan([FromQuery] DateTime? startDate)
    {
        var start = startDate?.Date ?? GetMondayOfWeek(DateTime.UtcNow);
        var end = start.AddDays(7);

        var mealPlans = await _context.MealPlans
            .Include(m => m.Recipe)
            .ThenInclude(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .Where(m => m.Date >= start && m.Date < end)
            .ToListAsync();

        var ingredientAmounts = new Dictionary<int, (Ingredient ingredient, decimal amount)>();
        foreach (var plan in mealPlans)
        {
            var ratio = (decimal)plan.Servings / plan.Recipe.Servings;
            foreach (var ri in plan.Recipe.Ingredients)
            {
                var needed = ri.Amount * ratio;
                if (ingredientAmounts.ContainsKey(ri.IngredientId))
                {
                    var existing = ingredientAmounts[ri.IngredientId];
                    ingredientAmounts[ri.IngredientId] = (existing.ingredient, existing.amount + needed);
                }
                else
                {
                    ingredientAmounts[ri.IngredientId] = (ri.Ingredient, needed);
                }
            }
        }

        // Subtract inventory
        var inventory = await _context.InventoryItems.ToListAsync();
        var inventoryDict = inventory.ToDictionary(i => i.IngredientId, i => i.CurrentStock);

        var newItems = new List<ShoppingListItem>();
        foreach (var (ingredientId, (ingredient, amount)) in ingredientAmounts)
        {
            var inStock = inventoryDict.GetValueOrDefault(ingredientId, 0);
            var toBuy = amount - inStock;
            if (toBuy > 0)
            {
                newItems.Add(new ShoppingListItem
                {
                    IngredientId = ingredientId,
                    Amount = toBuy,
                    IsChecked = false,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // Clear old unchecked items and add new
        var oldItems = await _context.ShoppingListItems.Where(s => !s.IsChecked).ToListAsync();
        _context.ShoppingListItems.RemoveRange(oldItems);
        _context.ShoppingListItems.AddRange(newItems);
        await _context.SaveChangesAsync();

        // Reload with includes
        var result = await _context.ShoppingListItems
            .Include(s => s.Ingredient)
            .OrderBy(s => s.Ingredient.Category)
            .ThenBy(s => s.Ingredient.Name)
            .ToListAsync();
        return Ok(result);
    }

    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> Toggle(int id)
    {
        var item = await _context.ShoppingListItems.FindAsync(id);
        if (item == null) return NotFound();
        item.IsChecked = !item.IsChecked;
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ShoppingListItems.FindAsync(id);
        if (item == null) return NotFound();
        _context.ShoppingListItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("clear-checked")]
    public async Task<IActionResult> ClearChecked()
    {
        var checked_items = await _context.ShoppingListItems.Where(s => s.IsChecked).ToListAsync();
        _context.ShoppingListItems.RemoveRange(checked_items);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static DateTime GetMondayOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}
