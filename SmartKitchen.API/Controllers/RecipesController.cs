using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/recipes")]
public class RecipesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RecipesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        return Ok(recipe);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Recipe recipe)
    {
        recipe.CreatedAt = DateTime.UtcNow;
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Recipe recipe)
    {
        var existing = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (existing == null) return NotFound();

        existing.Name = recipe.Name;
        existing.Description = recipe.Description;
        existing.Instructions = recipe.Instructions;
        existing.PrepTimeMinutes = recipe.PrepTimeMinutes;
        existing.CookTimeMinutes = recipe.CookTimeMinutes;
        existing.Servings = recipe.Servings;
        existing.Category = recipe.Category;
        existing.Difficulty = recipe.Difficulty;
        existing.ImageUrl = recipe.ImageUrl;
        existing.EstimatedCost = recipe.EstimatedCost;

        _context.RecipeIngredients.RemoveRange(existing.Ingredients);
        existing.Ingredients = recipe.Ingredients;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
