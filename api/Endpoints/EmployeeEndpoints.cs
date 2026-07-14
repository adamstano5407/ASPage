using APIKros.Requests.Employee;
using APIKros.Services;

namespace APIKros.Endpoints;

public static class EmployeeEndpoints
{
    public static IEndpointRouteBuilder MapEmployeeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/employees")
            .WithTags("Employees");

        group.MapGet("/", async (IEmployeeService service) =>
        {
            var employees = await service.GetAllAsync();
            return Results.Ok(employees);
        })
        .WithName("GetAllEmployees")
        .WithSummary("Get all employees")
        .WithDescription("Returns a list of all employees.")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IEmployeeService service) =>
        {
            var employee = await service.GetAsync(id);
            return Results.Ok(employee);
        })
        .WithName("GetEmployee")
        .WithSummary("Get employee by id")
        .WithDescription("Returns employee information.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateEmployeeRequest request, IEmployeeService service) =>
        {
            var employee = await service.CreateAsync(request);
            return Results.Created($"/api/employees/{employee.Id}", employee);
        })
        .WithName("CreateEmployee")
        .WithSummary("Create employee")
        .WithDescription("Creates a new employee.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (int id, UpdateEmployeeRequest request, IEmployeeService service) =>
        {
            await service.UpdateAsync(id, request);
            return Results.NoContent();
        })
        .WithName("UpdateEmployee")
        .WithSummary("Update employee")
        .WithDescription("Updates an existing employee.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IEmployeeService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteEmployee")
        .WithSummary("Delete employee")
        .WithDescription("Deletes an employee.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/company", async (int id, IEmployeeService service) =>
        {
            var company = await service.GetCompany(id);
            return Results.Ok(company);
        })
        .WithName("GetEmployeeCompany")
        .WithSummary("Get employee company")
        .WithDescription("Returns the company assigned to the employee.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/company", async (
            int id,
            ChangeCompanyRequest request,
            IEmployeeService service) =>
        {
            await service.ChangeCompany(id, request);
            return Results.NoContent();
        })
        .WithName("ChangeEmployeeCompany")
        .WithSummary("Change employee company")
        .WithDescription("Changes the employee company and removes leadership positions if needed.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}/leadership-positions", async (int id, IEmployeeService service) =>
        {
            await service.UnassignEmployeeFromLeadershipPositions(id);
            return Results.NoContent();
        })
        .WithName("UnassignEmployeeLeadershipPositions")
        .WithSummary("Unassign employee leadership positions")
        .WithDescription("Removes the employee from all manager/director positions.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}