using APIKros.Repositories;
using APIKros.Services;

namespace APIKros.Extensions;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IDivisionService, DivisionService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IDepartmentService, DepartmentService >();
        services.AddScoped<IEmployeeService, EmployeeService>();
        return services;
    }
}