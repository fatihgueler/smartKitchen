using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        var totalRecipes = await _context.Recipes.CountAsync();
        var totalIngredients = await _context.Ingredients.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var lowStockCount = await _context.InventoryItems
            .Where(i => i.CurrentStock <= i.MinimumStock)
            .CountAsync();
        var expiringCount = await _context.InventoryItems
            .Where(i => i.ExpiryDate != null && i.ExpiryDate <= DateTime.UtcNow.AddDays(7))
            .CountAsync();
        var todaysMeals = await _context.MealPlans
            .Include(m => m.Recipe)
            .Where(m => m.Date.Date == DateTime.UtcNow.Date)
            .OrderBy(m => m.MealType)
            .ToListAsync();
        var shoppingItemsCount = await _context.ShoppingListItems
            .Where(s => !s.IsChecked)
            .CountAsync();
        var recentOrders = await _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .ToListAsync();
        var inventoryItems = await _context.InventoryItems
            .Include(i => i.Ingredient)
            .CountAsync();

        return Ok(new
        {
            totalRecipes,
            totalIngredients,
            totalOrders,
            lowStockCount,
            expiringCount,
            todaysMeals,
            shoppingItemsCount,
            recentOrders,
            inventoryItems
        });
    }
}
