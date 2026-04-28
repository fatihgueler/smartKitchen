using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartKitchen.Domain;
using SmartKitchen.Infrastructure;

namespace SmartKitchen.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Recipe)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Recipe)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        order.CreatedAt = DateTime.UtcNow;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Order order)
    {
        var existing = await _context.Orders.FindAsync(id);
        if (existing == null) return NotFound();
        existing.CustomerName = order.CustomerName;
        existing.Status = order.Status;
        existing.Notes = order.Notes;
        existing.TotalPrice = order.TotalPrice;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
