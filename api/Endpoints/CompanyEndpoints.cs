using APIKros.Requests;
using APIKros.Services;

namespace APIKros.Endpoints;

public static class CompanyEndpoints
{
    public static IEndpointRouteBuilder MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/companies")
            .WithTags("Companies");

        group.MapGet("/", async (ICompanyService service) =>
            {
                var companies = await service.GetAllAsync();
                return Results.Ok(companies);
            })
            .WithName("GetAllCompanies")
            .WithSummary("Get all companies")
            .WithDescription("Returns a list of all companies.")
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, ICompanyService service) =>
            {
                var company = await service.GetAsync(id);

                return Results.Ok(company);
            })
            .WithName("GetCompany")
            .WithSummary("Get company by id")
            .WithDescription("Returns company information.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (CreateCompanyRequest request, ICompanyService service) =>
            {
                var company = await service.CreateAsync(request);
                return Results.Created($"/api/companies/{company.Id}", company);
            })
            .WithName("CreateCompany")
            .WithSummary("Create company")
            .WithDescription("Creates a new company.")
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", async (int id, UpdateCompanyRequest request, ICompanyService service) =>
            {
                await service.UpdateAsync(id, request);
                return Results.NoContent();
            })
            .WithName("UpdateCompany")
            .WithSummary("Update company")
            .WithDescription("Updates an existing company.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (int id, ICompanyService service) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteCompany")
            .WithSummary("Delete company")
            .WithDescription("Deletes a company.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/employees", async (int id, ICompanyService service) =>
            {
                var employees = await service.GetAllEmployees(id);
                return Results.Ok(employees);
            })
            .WithName("GetCompanyEmployees")
            .WithSummary("Get company employees")
            .WithDescription("Returns all employees belonging to the company.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id:int}/divisions", async (int id, ICompanyService service) =>
            {
                var divisions = await service.GetChildrenAsync(id);
                return Results.Ok(divisions);
            })
            .WithName("GetCompanyDivisions")
            .WithSummary("Get company divisions")
            .WithDescription("Returns all divisions belonging to the company.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/manager", async (
                int id,
                AssignManagerRequest request,
                ICompanyService service) =>
            {
                await service.AssignManagerAsync(id, request);
                return Results.NoContent();
            })
            .WithName("AssignCompanyManager")
            .WithSummary("Assign company manager")
            .WithDescription("Assigns a manager to the company.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}/manager", async (int id, ICompanyService service) =>
            {
                await service.UnassignManagerAsync(id);
                return Results.NoContent();
            })
            .WithName("UnassignCompanyManager")
            .WithSummary("Unassign company manager")
            .WithDescription("Removes the manager from the company.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}