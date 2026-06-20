using APIKros.Data;
using APIKros.DTOs.Department;
using APIKros.DTOs.Division;
using APIKros.DTOs.Project;
using APIKros.Models;
using APIKros.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers;

[ApiController]
[Route("api/divisions")]
public class DivisionController : ControllerBase
{
    private readonly AppDbContext _context;

    public DivisionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EndpointName("GetAllDivisions")]
    [EndpointSummary("Get all divisions")]
    [EndpointDescription("Returns a list of all divisions.")]
    [ProducesResponseType(typeof(IEnumerable<DivisionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var divisions = await _context.Divisions
            .Select(d => DivisionDto.CreateInstance(d))
            .ToListAsync();

        return Ok(divisions);
    }

    [HttpGet("{id:int}")]
    [EndpointName("GetDivisionById")]
    [EndpointSummary("Get division by ID")]
    [EndpointDescription("Returns basic information about a division.")]
    [ProducesResponseType(typeof(DivisionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var division = await _context.Divisions
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        return Ok(DivisionDto.CreateInstance(division));
    }

    [HttpPost]
    [EndpointName("CreateDivision")]
    [EndpointSummary("Create division")]
    [EndpointDescription("Creates a new division under an existing company.")]
    [ProducesResponseType(typeof(DivisionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDivisionRequest request)
    {
        var division = new Division
        {
            Name = request.Name,
            Code = request.Code,
            CompanyId = request.ParentId,
            ManagerId = request.ManagerId
        };

        _context.Divisions.Add(division);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = division.Id },
            DivisionDto.CreateInstance(division)
        );
    }

    [HttpPut("{id:int}")]
    [EndpointName("UpdateDivision")]
    [EndpointSummary("Update division")]
    [EndpointDescription("Updates an existing division by ID. Only provided fields are changed.")]
    [ProducesResponseType(typeof(DivisionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDivisionRequest request)
    {
        var division = await _context.Divisions
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        if (request.Name is not null)
            division.Name = request.Name;

        if (request.Code is not null)
            division.Code = request.Code;

        if (request.ManagerId.HasValue)
            division.ManagerId = request.ManagerId.Value;

        await _context.SaveChangesAsync();

        return Ok(DivisionDto.CreateInstance(division));
    }

    [HttpDelete("{id:int}")]
    [EndpointName("DeleteDivision")]
    [EndpointSummary("Delete division")]
    [EndpointDescription("Deletes an existing division from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> Delete(int id)
    {
        var division = await _context.Divisions
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        _context.Divisions.Remove(division);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{divisionId:int}/projects")]
    [EndpointName("DeleteDivisionProjects")]
    [EndpointSummary("Delete all division projects")]
    [EndpointDescription("Deletes all projects assigned to the specified division, including their departments.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDivisionProjects(int divisionId)
    {
        var divisionExists = await _context.Divisions
            .AnyAsync(d => d.Id == divisionId);

        if (!divisionExists)
            return NotFound();

        await _context.Projects
            .Where(p => p.DivisionId == divisionId)
            .ExecuteDeleteAsync();

        return NoContent();
    }

    [HttpGet("{id:int}/details")]
    [EndpointName("GetDivisionDetails")]
    [EndpointSummary("Get division details")]
    [EndpointDescription("Returns detailed information about a division, including company, manager and projects.")]
    [ProducesResponseType(typeof(DetailedDivisionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var division = await _context.Divisions
            .Include(d => d.Company)
            .Include(d => d.Manager)
            .Include(d => d.Projects)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        return Ok(DetailedDivisionDto.CreateInstance(division));
    }

    [HttpGet("{id:int}/projects")]
    [EndpointName("GetDivisionProjects")]
    [EndpointSummary("Get division projects")]
    [EndpointDescription("Returns all projects that belong to the specified division.")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjects(int id)
    {
        var division = await _context.Divisions
            .Include(d => d.Projects)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        var projectsDto = division.Projects
            .Select(ProjectDto.CreateInstance)
            .ToList();

        return Ok(projectsDto);
    }

    [HttpGet("{id:int}/departments")]
    [EndpointName("GetDivisionDepartments")]
    [EndpointSummary("Get division departments")]
    [EndpointDescription("Returns all departments from all projects of the specified division.")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartments(int id)
    {
        var division = await _context.Divisions
            .Include(d => d.Projects)
                .ThenInclude(p => p.Departments)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        var departmentsDto = division.Projects
            .SelectMany(p => p.Departments)
            .Select(DepartmentDto.CreateInstance)
            .ToList();

        return Ok(departmentsDto);
    }

    [HttpPut("{id:int}/manager")]
    [EndpointName("AssignDivisionManager")]
    [EndpointSummary("Assign division manager")]
    [EndpointDescription("Assigns an existing employee as the manager of a division. The employee must belong to the same company as the division.")]
    [ProducesResponseType(typeof(DivisionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignManager(
        int id,
        [FromBody] AssignManagerRequest request)
    {
        var division = await _context.Divisions
            .FirstOrDefaultAsync(d => d.Id == id);

        if (division is null)
            return NotFound();

        var employeeBelongsToCompany = await _context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId &&
                           e.CompanyId == division.CompanyId);

        if (!employeeBelongsToCompany)
            return BadRequest("Employee must belong to the same company as the division.");

        division.ManagerId = request.EmployeeId;

        await _context.SaveChangesAsync();

        return Ok(DivisionDto.CreateInstance(division));
    }
}