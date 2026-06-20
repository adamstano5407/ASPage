using APIKros.Data;
using APIKros.DTOs;
using APIKros.DTOs.Employee;
using APIKros.Models;
using APIKros.Requests.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointName("GetAllEmployees")]
        [EndpointSummary("Get all employees")]
        [EndpointDescription("Returns a list of all employees")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.Employees
             .Select(e => EmployeeDto.CreateInstance(e))
             .ToListAsync();

            return Ok(employees);
        }


        [HttpGet("{id}")]
        [EndpointName("GetEmployeeDetail")]
        [EndpointSummary("Get employee by ID")]
        [EndpointDescription("Returns a single employee by the specified identifier.")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(EmployeeDto.CreateInstance(employee));
        }

        [HttpPost(Name = "CreateEmployee")]
        [EndpointName("CreateEmployee")]
        [EndpointSummary("Create a new employee")]
        [EndpointDescription(
            "Creates a new employee and assigns them to an existing company. " +
            "The employee record includes personal and contact information such as title, name, email, and phone number. " +
            "Returns the created employee with its generated identifier."
        )]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
        {
            var employee = new Employee
            {
                Title = request.Title,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                CompanyId = request.CompanyId
            };
                
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.Id },
                EmployeeDto.CreateInstance(employee)
            );
        }

        [HttpPut("{id}")]
        [EndpointName("UpdateEmployee")]
        [EndpointSummary("Update employee")]
        [EndpointDescription("Updates an existing employee by ID. Only provided fields are changed.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            var targetCompanyId = request.CompanyId ?? employee.CompanyId;

            if (request.EmployeeNumber is not null)
            {
                var employeeNumberExists = await _context.Employees
                    .AnyAsync(e =>
                        e.CompanyId == targetCompanyId &&
                        e.EmployeeNumber == request.EmployeeNumber &&
                        e.Id != id);

                if (employeeNumberExists)
                    return BadRequest("Employee number already exists in this company.");

                employee.EmployeeNumber = request.EmployeeNumber;
            }
            
            if (request.Title is not null)
                employee.Title = request.Title;

            if (request.FirstName is not null)
                employee.FirstName = request.FirstName;

            if (request.LastName is not null)
                employee.LastName = request.LastName;

            if (request.Email is not null)
            {
                var emailExists = await _context.Employees
                    .AnyAsync(e => e.Email == request.Email && e.Id != id);

                if (emailExists)
                    return BadRequest("Employee with this email already exists.");

                employee.Email = request.Email;
            }

            if (request.Phone is not null)
                employee.Phone = request.Phone;

            if (request.CompanyId.HasValue && request.CompanyId.Value != employee.CompanyId)
            {
                await UnassignEmployeeFromLeadershipPositions(employee.Id);
                employee.CompanyId = request.CompanyId.Value;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        [EndpointName("DeleteEmployee")]
        [EndpointSummary("Delete employee")]
        [EndpointDescription("Delete an existing employee out of company. That means he will be removed out of manager positions as well.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee is null)
                return NotFound("Employee not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        

        [HttpPut("change-company")]
        [EndpointName("ChangeEmployeeCompany")]
        [EndpointSummary("Change employee company")]
        [EndpointDescription(
            "Moves an employee to another company. " +
            "If the employee is assigned as a director, division manager, project manager, or department manager, " +
            "these leadership assignments are removed before changing the company."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeCompany([FromBody] ChangeCompanyRequest request)
        {
            var employee = await _context.Employees.FindAsync(request.EmployeeId);

            
            await UnassignEmployeeFromLeadershipPositions(request.EmployeeId);
            employee!.CompanyId = request.NewCompanyId;
            

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("company/{companyId:int}")]
        [EndpointName("GetEmployeesByCompanyId")]
        [EndpointSummary("Get employees by Company ID")]
        [EndpointDescription("Returns employees that belong to the specified company.")]
        [ProducesResponseType(typeof(List<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCompanyId(int companyId)
        {
            var employees = await _context.Employees
                .Where(e => e.CompanyId == companyId)
                .ToListAsync();

            return Ok(employees.Select(EmployeeDto.CreateInstance));
        }
        
        
        
        private async Task UnassignEmployeeFromLeadershipPositions(int employeeId)
        {
            await _context.Companies
                .Where(c => c.ManagerId == employeeId)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.ManagerId, (int?)null));

            await _context.Divisions
                .Where(d => d.ManagerId == employeeId)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.ManagerId, (int?)null));

            await _context.Projects
                .Where(p => p.ManagerId == employeeId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.ManagerId, (int?)null));

            await _context.Departments
                .Where(d => d.ManagerId == employeeId)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.ManagerId, (int?)null));
        }
    }
    
    
}