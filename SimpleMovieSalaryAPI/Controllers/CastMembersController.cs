using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMovieSalaryAPI.Data;
using SimpleMovieSalaryAPI.Interfaces;
using SimpleMovieSalaryAPI.Models;
using SimpleMovieSalaryAPI.Services;
// other using statements...

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all endpoints by default
public class CastMembersController : ControllerBase
{
    private readonly ICastNewService _castService;
    public CastMembersController(ICastNewService castService)
    {
        _castService = castService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CastMember>>> GetAll()
    {
        var members = await _castService.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CastMember>> Get(int id)
    {
        var member = await _castService.GetByIdAsync(id);
        if (member == null) return NotFound();
        return Ok(member);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Only Admins can create
    public async Task<ActionResult<CastMember>> Create(CastMember member)
    {
        var created = await _castService.CreateAsync(member);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can update
    public async Task<IActionResult> Update(int id, CastMember updated)
    {
        var updatedSuccess = await _castService.UpdateAsync(id, updated);
        if (!updatedSuccess) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can delete
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _castService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
