using APIKros.Data;
using APIKros.DTOs.Department;
using APIKros.Models;
using APIKros.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EndpointName("GetAllDepartments")]
    [EndpointSummary("Get all departments")]
    [EndpointDescription("Returns a list of all departments.")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _context.Departments
            .Select(d => DepartmentDto.CreateInstance(d))
            .ToListAsync();

        return Ok(departments);
    }

    [HttpGet("{id:int}")]
    [EndpointName("GetDepartmentById")]
    [EndpointSummary("Get department by ID")]
    [EndpointDescription("Returns basic information about a department.")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return NotFound();

        return Ok(DepartmentDto.CreateInstance(department));
    }

    [HttpPost]
    [EndpointName("CreateDepartment")]
    [EndpointSummary("Create department")]
    [EndpointDescription("Creates a new department under an existing project.")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        var department = new Department
        {
            Name = request.Name,
            Code = request.Code,
            ProjectId = request.ParentId,
            ManagerId = request.ManagerId
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            DepartmentDto.CreateInstance(department)
        );
    }

    [HttpPut("{id:int}")]
    [EndpointName("UpdateDepartment")]
    [EndpointSummary("Update department")]
    [EndpointDescription("Updates an existing department by ID. Only provided fields are changed.")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentRequest request)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return NotFound();

        if (request.Name is not null)
            department.Name = request.Name;

        if (request.Code is not null)
            department.Code = request.Code;

        if (request.ManagerId.HasValue)
            department.ManagerId = request.ManagerId.Value;

        await _context.SaveChangesAsync();

        return Ok(DepartmentDto.CreateInstance(department));
    }

    [HttpDelete("{id:int}")]
    [EndpointName("DeleteDepartment")]
    [EndpointSummary("Delete department")]
    [EndpointDescription("Deletes an existing department from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return NotFound();

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id:int}/details")]
    [EndpointName("GetDepartmentDetails")]
    [EndpointSummary("Get department details")]
    [EndpointDescription("Returns detailed information about a department, including project and manager.")]
    [ProducesResponseType(typeof(DetailedDepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Project)
            .Include(d => d.Manager)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return NotFound();

        return Ok(DetailedDepartmentDto.CreateInstance(department));
    }

    [HttpPut("{id:int}/manager")]
    [EndpointName("AssignDepartmentManager")]
    [EndpointSummary("Assign department manager")]
    [EndpointDescription("Assigns an existing employee as the manager of a department. The employee must belong to the same company as the department.")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> AssignManager(
        int id,
        [FromBody] AssignManagerRequest request)
    {
        var department = await _context.Departments
            .Include(d => d.Project)
                .ThenInclude(p => p.Division)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return NotFound();

        var employeeBelongsToCompany = await _context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId &&
                           department.Project != null &&
                           e.CompanyId == department.Project.Division.CompanyId);

        if (!employeeBelongsToCompany)
            return BadRequest("Employee must belong to the same company as the department.");

        department.ManagerId = request.EmployeeId;

        await _context.SaveChangesAsync();

        return Ok(DepartmentDto.CreateInstance(department));
    }
}