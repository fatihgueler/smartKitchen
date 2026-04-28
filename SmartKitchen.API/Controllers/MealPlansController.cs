using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/mealplans")]
public class MealPlansController : ControllerBase
{
    private readonly AppDbContext _context;

    public MealPlansController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _context.MealPlans
            .Include(m => m.Recipe)
            .OrderBy(m => m.Date)
            .ThenBy(m => m.MealType)
            .ToListAsync();
        return Ok(plans);
    }

    [HttpGet("week")]
    public async Task<IActionResult> GetWeek([FromQuery] DateTime? startDate)
    {
        var start = startDate?.Date ?? GetMondayOfWeek(DateTime.UtcNow);
        var end = start.AddDays(7);
        var plans = await _context.MealPlans
            .Include(m => m.Recipe)
            .Where(m => m.Date >= start && m.Date < end)
            .OrderBy(m => m.Date)
            .ThenBy(m => m.MealType)
            .ToListAsync();
        return Ok(plans);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MealPlan plan)
    {
        _context.MealPlans.Add(plan);
        await _context.SaveChangesAsync();
        return Ok(plan);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MealPlan plan)
    {
        var existing = await _context.MealPlans.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Date = plan.Date;
        existing.MealType = plan.MealType;
        existing.RecipeId = plan.RecipeId;
        existing.Servings = plan.Servings;
        existing.Notes = plan.Notes;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await _context.MealPlans.FindAsync(id);
        if (plan == null) return NotFound();
        _context.MealPlans.Remove(plan);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static DateTime GetMondayOfWeek(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}
