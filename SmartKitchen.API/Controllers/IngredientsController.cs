using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public IngredientsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var ingredients = await _context.Ingredients
            .OrderBy(i => i.Name)
            .ToListAsync();
        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return NotFound();
        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = ingredient.Id }, ingredient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ingredient ingredient)
    {
        var existing = await _context.Ingredients.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = ingredient.Name;
        existing.Unit = ingredient.Unit;
        existing.Category = ingredient.Category;
        existing.PricePerUnit = ingredient.PricePerUnit;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return NotFound();
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
