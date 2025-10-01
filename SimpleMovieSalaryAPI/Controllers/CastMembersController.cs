using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
    private readonly ILoggerService _logger;
    public CastMembersController(ICastNewService castService, ILoggerService logger)
    {
        _castService = castService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CastMember>>> GetAll()
    {
        _logger.LogInfo("Getting all cast members");
        var members = await _castService.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CastMember>> Get(int id)
    {
        _logger.LogInfo($"Getting cast member with ID {id}");
        var member = await _castService.GetByIdAsync(id);
        if (member == null)
        {
            _logger.LogWarning($"Cast member with ID {id} not found");
            return NotFound();
        }
        return Ok(member);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CastMember>>> SearchByQuery([FromQuery] string filter)
    {
        _logger.LogInfo($"Searching cast members using filter: {filter}");

        int? id = null;
        string? name = null;
        decimal? remuneration = null;

        try
        {
            var parameters = filter.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var param in parameters)
            {
                var keyValue = param.Split('=', 2);
                if (keyValue.Length != 2) continue;

                var key = keyValue[0].Trim().ToLower();
                var value = keyValue[1].Trim();

                switch (key)
                {
                    case "id":
                        if (int.TryParse(value, out var parsedId)) id = parsedId;
                        break;
                    case "name":
                        name = value;
                        break;
                    case "remuneration":
                        if (decimal.TryParse(value, out var parsedRem)) remuneration = parsedRem;
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error parsing filter string: {ex.Message}");
            return BadRequest("Invalid filter format.");
        }

        var results = await _castService.SearchByParamsAsync(id, name, remuneration);
        return Ok(results);
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Owner")] // Only Admins can create
    public async Task<ActionResult<CastMember>> Create(CastMember member)
    {
        _logger.LogInfo($"Creating new cast member");
        var created = await _castService.CreateAsync(member);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Owner")] // Only Admins can update
    public async Task<IActionResult> Update(int id, CastMember updated)
    {
        _logger.LogInfo($"Updating existing cast member with ID {id}");
        var updatedSuccess = await _castService.UpdateAsync(id, updated);
        if (!updatedSuccess)
        {
            _logger.LogError($"Cast member with ID: {id} not found.");
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")] // Only Admins can delete
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInfo($"Removing existing cast member with ID {id}");
        var deleted = await _castService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogError($"Cast member with ID: {id} not found.");
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcel()
    {
        var fileContents = await _castService.ExportToExcelAsync();

        return File(fileContents,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "CastMembers.xlsx");
    }

}
