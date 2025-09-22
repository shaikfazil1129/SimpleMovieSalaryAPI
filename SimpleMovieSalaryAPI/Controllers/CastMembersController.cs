using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMovieSalaryAPI.Data;
using SimpleMovieSalaryAPI.Models;
// other using statements...

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints by default
public class CastMembersController : ControllerBase
{
    private readonly AppDbContext _context;
    public CastMembersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CastMember>>> GetAll()
    {
        return await _context.CastMembers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CastMember>> Get(int id)
    {
        var cast = await _context.CastMembers.FindAsync(id);
        if (cast == null) return NotFound();
        return Ok(cast);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Only Admins can create
    public async Task<ActionResult<CastMember>> Create(CastMember member)
    {
        _context.CastMembers.Add(member);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = member.Id }, member);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can update
    public async Task<IActionResult> Update(int id, CastMember updated)
    {
        if (id != updated.Id) return BadRequest();
        var existing = await _context.CastMembers.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = updated.Name;
        existing.Remuneration = updated.Remuneration;
        existing.AmountPaid = updated.AmountPaid;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can delete
    public async Task<IActionResult> Delete(int id)
    {
        var cast = await _context.CastMembers.FindAsync(id);
        if (cast == null) return NotFound();
        _context.CastMembers.Remove(cast);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
