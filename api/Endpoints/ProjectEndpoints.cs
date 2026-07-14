using APIKros.Requests;
using APIKros.Services;

namespace APIKros.Endpoints;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/projects")
            .WithTags("Projects");

        group.MapGet("/", async (IProjectService service) =>
        {
            var projects = await service.GetAllAsync();
            return Results.Ok(projects);
        })
        .WithName("GetAllProjects")
        .WithSummary("Get all projects")
        .WithDescription("Returns a list of all projects.")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, IProjectService service) =>
        {
            var project = await service.GetAsync(id);
            return Results.Ok(project);
        })
        .WithName("GetProject")
        .WithSummary("Get project by id")
        .WithDescription("Returns project information.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateProjectRequest request, IProjectService service) =>
        {
            var project = await service.CreateAsync(request);
            return Results.Created($"/api/projects/{project.Id}", project);
        })
        .WithName("CreateProject")
        .WithSummary("Create project")
        .WithDescription("Creates a new project.")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (int id, UpdateProjectRequest request, IProjectService service) =>
        {
            await service.UpdateAsync(id, request);
            return Results.NoContent();
        })
        .WithName("UpdateProject")
        .WithSummary("Update project")
        .WithDescription("Updates an existing project.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, IProjectService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteProject")
        .WithSummary("Delete project")
        .WithDescription("Deletes a project.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/departments", async (int id, IProjectService service) =>
        {
            var departments = await service.GetChildrenAsync(id);
            return Results.Ok(departments);
        })
        .WithName("GetProjectDepartments")
        .WithSummary("Get project departments")
        .WithDescription("Returns all departments belonging to the project.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/parent", async (int id, IProjectService service) =>
        {
            var parent = await service.GetParentAsync(id);
            return Results.Ok(parent);
        })
        .WithName("GetProjectParent")
        .WithSummary("Get parent division")
        .WithDescription("Returns the division that owns the project.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/manager", async (int id, AssignManagerRequest request, IProjectService service) =>
        {
            await service.AssignManagerAsync(id, request);
            return Results.NoContent();
        })
        .WithName("AssignProjectManager")
        .WithSummary("Assign project manager")
        .WithDescription("Assigns a manager to the project.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}/manager", async (int id, IProjectService service) =>
        {
            await service.UnassignManagerAsync(id);
            return Results.NoContent();
        })
        .WithName("UnassignProjectManager")
        .WithSummary("Unassign project manager")
        .WithDescription("Removes the manager from the project.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}