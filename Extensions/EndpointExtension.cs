using APIKros.Endpoints;

namespace APIKros.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder app)
    {
        app.MapCompanyEndpoints();
        app.MapDivisionEndpoints();
        app.MapProjectEndpoints();
        app.MapDepartmentEndpoints();
        app.MapEmployeeEndpoints();

        return app;
    }
}