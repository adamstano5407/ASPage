using APIKros.Requests;
using APIKros.Services;

namespace APIKros.Endpoints;

public static class DivisionEndpoints
{
    public static IEndpointRouteBuilder MapDivisionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/divisions")
            .WithTags("Divisions");

        group.MapGet("/", async (IDivisionService service) =>
        {
            var divisions = await service.GetAllAsync();
            return Results.Ok(divisions);
        })
        .WithName("GetAllDivisions")
        .WithSummary("Get all divisions")
        .WithDescription("Returns a list of all divisions.")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IDivisionService service) =>
        {
            var division = await service.GetAsync(id);
            return Results.Ok(division);
        })
        .WithName("GetDivision")
        .WithSummary("Get division by id")
        .WithDescription("Returns division information.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateDivisionRequest request, IDivisionService service) =>
        {
            var division = await service.CreateAsync(request);
            return Results.Created($"/api/divisions/{division.Id}", division);
        })
        .WithName("CreateDivision")
        .WithSummary("Create division")
        .WithDescription("Creates a new division.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (
            int id,
            UpdateDivisionRequest request,
            IDivisionService service) =>
        {
            await service.UpdateAsync(id, request);
            return Results.NoContent();
        })
        .WithName("UpdateDivision")
        .WithSummary("Update division")
        .WithDescription("Updates an existing division.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IDivisionService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteDivision")
        .WithSummary("Delete division")
        .WithDescription("Deletes a division.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/projects", async (int id, IDivisionService service) =>
        {
            var projects = await service.GetChildrenAsync(id);
            return Results.Ok(projects);
        })
        .WithName("GetDivisionProjects")
        .WithSummary("Get division projects")
        .WithDescription("Returns all projects belonging to the division.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/parent", async (int id, IDivisionService service) =>
        {
            var company = await service.GetParentAsync(id);
            return Results.Ok(company);
        })
        .WithName("GetDivisionParent")
        .WithSummary("Get parent company")
        .WithDescription("Returns the company that owns the division.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/manager", async (
            int id,
            AssignManagerRequest request,
            IDivisionService service) =>
        {
            await service.AssignManagerAsync(id, request);
            return Results.NoContent();
        })
        .WithName("AssignDivisionManager")
        .WithSummary("Assign division manager")
        .WithDescription("Assigns a manager to the division.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}/manager", async (int id, IDivisionService service) =>
        {
            await service.UnassignManagerAsync(id);
            return Results.NoContent();
        })
        .WithName("UnassignDivisionManager")
        .WithSummary("Unassign division manager")
        .WithDescription("Removes the manager from the division.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}