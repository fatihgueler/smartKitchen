using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public InventoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.InventoryItems
            .Include(i => i.Ingredient)
            .OrderBy(i => i.Ingredient.Name)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var items = await _context.InventoryItems
            .Include(i => i.Ingredient)
            .Where(i => i.CurrentStock <= i.MinimumStock)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("expiring")]
    public async Task<IActionResult> GetExpiring()
    {
        var threshold = DateTime.UtcNow.AddDays(7);
        var items = await _context.InventoryItems
            .Include(i => i.Ingredient)
            .Where(i => i.ExpiryDate != null && i.ExpiryDate <= threshold)
            .OrderBy(i => i.ExpiryDate)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryItem item)
    {
        item.LastUpdated = DateTime.UtcNow;
        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InventoryItem item)
    {
        var existing = await _context.InventoryItems.FindAsync(id);
        if (existing == null) return NotFound();
        existing.CurrentStock = item.CurrentStock;
        existing.MinimumStock = item.MinimumStock;
        existing.ExpiryDate = item.ExpiryDate;
        existing.LastUpdated = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null) return NotFound();
        _context.InventoryItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
