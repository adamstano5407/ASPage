using APIKros.Data;
using APIKros.Models;
using Microsoft.AspNetCore.Mvc;
using APIKros.DTOs.Company;
using APIKros.DTOs.Department;
using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;
using APIKros.DTOs.Project;
using APIKros.Requests;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompanyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EndpointName("GetAllCompanies")]
    [EndpointSummary("Get all companies")]
    [EndpointDescription("Returns a list of all companies")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _context.Companies
            .Select(c => CompanyDto.CreateInstance(c))
            .ToListAsync();

        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    [EndpointName("GetCompanyDetail")]
    [EndpointSummary("Get Company By Id")]
    [EndpointDescription("Return Company with basic info")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(CompanyDto.CreateInstance(company));
    }

    [HttpPost]
    [EndpointName("CreateCompany")]
    [EndpointSummary("Create company")]
    [EndpointDescription("Creates a new company and optionally assigns an existing employee as the company director.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Code = dto.Code,

        };

        if (dto.ManagerId.HasValue)
            company.ManagerId = dto.ManagerId.Value;

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = company.Id },
            CompanyDto.CreateInstance(company)
        );
    }
    
    [HttpPut("{id:int}")]
    [EndpointName("UpdateCompany")]
    [EndpointSummary("Update company")]
    [EndpointDescription("Updates an existing company by ID. Only provided fields are changed.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyRequest dto)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        if (dto.Name is not null)
            company.Name = dto.Name;

        if (dto.Code is not null)
            company.Code = dto.Code;

        if (dto.ManagerId.HasValue)
            company.ManagerId = dto.ManagerId.Value;

        await _context.SaveChangesAsync();

        return Ok(CompanyDto.CreateInstance(company));
    }

    [HttpDelete("{id:int}")]
    [EndpointName("DeleteCompany")]
    [EndpointSummary("Delete company")]
    [EndpointDescription("Deletes an existing company from the system.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    
    [HttpDelete("{companyId:int}/divisions")]
    [EndpointName("DeleteCompanyDivisions")]
    [EndpointSummary("Delete all company divisions")]
    [EndpointDescription("Deletes all divisions assigned to the specified company.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCompanyDivisions(int companyId)
    {
        var companyExists = await _context.Companies
            .AnyAsync(c => c.Id == companyId);

        if (!companyExists)
            return NotFound();

        await _context.Divisions
            .Where(d => d.CompanyId == companyId)
            .ExecuteDeleteAsync();

        return NoContent();
    }

    [HttpGet("{id:int}/details")]
    [EndpointName("GetCompanyDetails")]
    [EndpointSummary("Get company details")]
    [EndpointDescription("Returns detailed information about a company, including director, divisions, and employees.")]
    [ProducesResponseType(typeof(DetailedCompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetails(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Manager)
            .Include(c => c.Divisions)
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(DetailedCompanyDto.CreateInstance(company));
    }


    [HttpGet("{id:int}/structured")]
    [EndpointName("GetCompanyStructured")]
    [EndpointSummary("Get company structure")]
    [EndpointDescription("Returns the full organizational structure of a company, including divisions, projects, departments, and managers.")]
    [ProducesResponseType(typeof(StructuredCompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStructured(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Manager)
            .Include(c => c.Employees)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Manager)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Manager)

            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Departments)
                        .ThenInclude(dep => dep.Manager)

            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        return Ok(StructuredCompanyDto.CreateInstance(company));
    }

    [HttpGet("{id:int}/employees")]
    [EndpointName("GetCompanyEmployees")]
    [EndpointSummary("Get company employees")]
    [EndpointDescription("Returns all employees assigned to the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployees(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Employees)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var employeesDto = company.Employees
            .Select(EmployeeDto.CreateInstance)
            .ToList();
        return Ok(employeesDto);
    }

    [HttpGet("{id:int}/divisions")]
    [EndpointName("GetCompanyDivisions")]
    [EndpointSummary("Get company divisions")]
    [EndpointDescription("Returns all divisions that belong to the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDivisions(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var divisionsDto = company.Divisions
            .Select(DivisionDto.CreateInstance)
            .ToList();
        return Ok(divisionsDto);
    }
    
    [HttpGet("{id:int}/projects")]
    [EndpointName("GetCompanyProjects")]
    [EndpointSummary("Get company projects")]
    [EndpointDescription("Returns all projects from all divisions of the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjects(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var projectsDto = company.Divisions
            .SelectMany(d => d.Projects)
            .Select(ProjectDto.CreateInstance)
            .ToList();
        return Ok(projectsDto);
    }
    
    [HttpGet("{id:int}/departments")]
    [EndpointName("GetCompanyDepartments")]
    [EndpointSummary("Get company departments")]
    [EndpointDescription("Returns all departments from all projects and divisions of the specified company.")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepartments(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Divisions)
                .ThenInclude(d => d.Projects)
                    .ThenInclude(p => p.Departments)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (company is null)
            return NotFound();
        var departmentsDto = company.Divisions
            .SelectMany(d => d.Projects)
            .SelectMany(p => p.Departments)
            .Select(DepartmentDto.CreateInstance)
            .ToList();
        return Ok(departmentsDto);
    }

    [HttpPut("{id:int}/director")]
    [EndpointName("AssignCompanyDirector")]
    [EndpointSummary("Assign company director")]
    [EndpointDescription("Assigns an existing employee as the director of a company. The employee must belong to the specified company.")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignDirector(
        int id,
        [FromBody] AssignManagerRequest request)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company is null)
            return NotFound();

        var employeeBelongsToCompany = await _context.Employees
            .AnyAsync(e => e.Id == request.EmployeeId && e.CompanyId == id);

        if (!employeeBelongsToCompany)
            return BadRequest("Employee must belong to the specified company.");

        company.ManagerId = request.EmployeeId;

        await _context.SaveChangesAsync();

        return Ok(CompanyDto.CreateInstance(company));
    }
}