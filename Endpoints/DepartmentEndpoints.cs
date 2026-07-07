using APIKros.Requests;
using APIKros.Services;

namespace APIKros.Endpoints;

public static class DepartmentEndpoints
{
    public static IEndpointRouteBuilder MapDepartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/departments")
            .WithTags("Departments");

        group.MapGet("/", async (IDepartmentService service) =>
        {
            var departments = await service.GetAllAsync();
            return Results.Ok(departments);
        })
        .WithName("GetAllDepartments")
        .WithSummary("Get all departments")
        .WithDescription("Returns a list of all departments.")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IDepartmentService service) =>
        {
            var department = await service.GetAsync(id);
            return Results.Ok(department);
        })
        .WithName("GetDepartment")
        .WithSummary("Get department by id")
        .WithDescription("Returns department information.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateDepartmentRequest request, IDepartmentService service) =>
        {
            var department = await service.CreateAsync(request);
            return Results.Created($"/api/departments/{department.Id}", department);
        })
        .WithName("CreateDepartment")
        .WithSummary("Create department")
        .WithDescription("Creates a new department.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (int id, UpdateDepartmentRequest request, IDepartmentService service) =>
        {
            await service.UpdateAsync(id, request);
            return Results.NoContent();
        })
        .WithName("UpdateDepartment")
        .WithSummary("Update department")
        .WithDescription("Updates an existing department.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IDepartmentService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteDepartment")
        .WithSummary("Delete department")
        .WithDescription("Deletes a department.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/parent", async (int id, IDepartmentService service) =>
        {
            var parent = await service.GetParentAsync(id);
            return Results.Ok(parent);
        })
        .WithName("GetDepartmentParent")
        .WithSummary("Get parent project")
        .WithDescription("Returns the project that owns the department.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/manager", async (int id, AssignManagerRequest request, IDepartmentService service) =>
        {
            await service.AssignManagerAsync(id, request);
            return Results.NoContent();
        })
        .WithName("AssignDepartmentManager")
        .WithSummary("Assign department manager")
        .WithDescription("Assigns a manager to the department.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}/manager", async (int id, IDepartmentService service) =>
        {
            await service.UnassignManagerAsync(id);
            return Results.NoContent();
        })
        .WithName("UnassignDepartmentManager")
        .WithSummary("Unassign department manager")
        .WithDescription("Removes the manager from the department.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}