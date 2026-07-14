using APIKros.Data;
using APIKros.Exceptions;
using APIKros.Seeders.Fakers;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Companies.AnyAsync())
            throw new DatabaseNotEmpty();

        var companies = new CompanyFaker().Generate(3);

        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();

        foreach (var company in companies)
        {
            var employees = new EmployeeFaker(company.Id).Generate(10);

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            company.AssignManager(employees[0].Id);

            var divisions = new DivisionFaker(company.Id, employees).Generate(3);

            context.Divisions.AddRange(divisions);
            await context.SaveChangesAsync();

            foreach (var division in divisions)
            {
                var projects = new ProjectFaker(division.Id, employees).Generate(3);

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                foreach (var project in projects)
                {
                    var departments = new DepartmentFaker(project.Id, employees).Generate(3);
                    context.Departments.AddRange(departments);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}