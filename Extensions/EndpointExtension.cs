using APIKros.Endpoints;

namespace APIKros.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        if (!app.ServiceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            api.RequireRateLimiting("default");
        }
        app.MapCompanyEndpoints();
        app.MapDivisionEndpoints();
        app.MapProjectEndpoints();
        app.MapDepartmentEndpoints();
        app.MapEmployeeEndpoints();

        return app;
    }
}