using ClosedXML.Excel;
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
        var castMembers = await _castService.GetAllAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("CastMembers");

        // Add headers
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Remuneration";
        worksheet.Cell(1, 4).Value = "Amount Paid";
        worksheet.Cell(1, 5).Value = "Remaining Amount";
        worksheet.Cell(1, 6).Value = "Status";

        int row = 2;
        foreach (var member in castMembers)
        {
            worksheet.Cell(row, 1).Value = member.Id;
            worksheet.Cell(row, 2).Value = member.Name;
            worksheet.Cell(row, 3).Value = member.Remuneration;
            worksheet.Cell(row, 4).Value = member.AmountPaid;
            worksheet.Cell(row, 5).Value = member.RemainingAmount;
            worksheet.Cell(row, 6).Value = member.Status;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "CastMembers.xlsx");
    }

}
