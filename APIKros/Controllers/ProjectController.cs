using APIKros.Data;
using APIKros.DTOs.Department;
using APIKros.DTOs.Project;
using APIKros.Models;
using APIKros.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EndpointName("GetAllProjects")]
    [EndpointSummary("Get all projects")]
    [EndpointDescription("Returns a list of all projects.")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _context.Projects
            .Select(p => ProjectDto.CreateInstance(p))
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    [EndpointName("GetProjectById")]
    [EndpointSummary("Get project by ID")]
    [EndpointDescription("Returns basic information about a project.")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        return Ok(ProjectDto.CreateInstance(project));
    }

    [HttpPost]
    [EndpointName("CreateProject")]
    [EndpointSummary("Create project")]
    [EndpointDescription("Creates a new project under an existing division.")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
    {
        var project = new Project
        {
            Name = request.Name,
            Code = request.Code,
            DivisionId = request.ParentId,
            ManagerId = request.ManagerId
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = project.Id },
            ProjectDto.CreateInstance(project)
        );
    }

    [HttpPut("{id:int}")]
    [EndpointName("UpdateProject")]
    [EndpointSummary("Update project")]
    [EndpointDescription("Updates an existing project by ID. Only provided fields are changed.")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectRequest request)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        if (request.Name is not null)
            project.Name = request.Name;

        if (request.Code is not null)
            project.Code = request.Code;

        if (request.ManagerId.HasValue)
            project.ManagerId = request.ManagerId.Value;

        await _context.SaveChangesAsync();

        return Ok(ProjectDto.CreateInstance(project));
    }

    [HttpDelete("{id:int}")]
    [EndpointName("DeleteProject")]
    [EndpointSummary("Delete project")]
    [EndpointDescription("Deletes an existing project from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{projectId:int}/departments")]
    [EndpointName("DeleteProjectDepartments")]
    [EndpointSummary("Delete all project departments")]
    [EndpointDescription("Deletes all departments assigned to the specified project.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProjectDepartments(int projectId)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId);

        if (!projectExists)
            return NotFound();

        await _context.Departments
            .Where(d => d.ProjectId == projectId)
            .ExecuteDeleteAsync();

        return NoContent();
    }

    [HttpGet("{id:int}/details")]
    [EndpointName("GetProjectDetails")]
    [EndpointSummary("Get project details")]
    [EndpointDescription("Returns detailed information about a project, including division, manager and departments.")]
    [ProducesResponseType(typeof(DetailedProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Division)
            .Include(p => p.Manager)
            .Include(p => p.Departments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        return Ok(DetailedProjectDto.CreateInstance(project));
    }

    [HttpGet("{id:int}/departments")]
    [EndpointName("GetProjectDepartments")]
    [EndpointSummary("Get project departments")]
    [EndpointDescription("Returns all departments that belong to the specified project.")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartments(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Departments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        var departmentsDto = project.Departments
            .Select(DepartmentDto.CreateInstance)
            .ToList();

        return Ok(departmentsDto);
    }

    [HttpPut("{id:int}/manager")]
    [EndpointName("AssignProjectManager")]
    [EndpointSummary("Assign project manager")]
    [EndpointDescription("Assigns an existing employee as the manager of a project. The employee must belong to the same company as the project.")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignManager(
        int id,
        [FromBody] AssignManagerRequest request)
    {
        var project = await _context.Projects
            .Include(p => p.Division)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return NotFound();

        var employeeBelongsToCompany = await _context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId &&
                           e.CompanyId == project.Division.CompanyId);

        if (!employeeBelongsToCompany)
            return BadRequest("Employee must belong to the same company as the project.");

        project.ManagerId = request.EmployeeId;

        await _context.SaveChangesAsync();

        return Ok(ProjectDto.CreateInstance(project));
    }
}