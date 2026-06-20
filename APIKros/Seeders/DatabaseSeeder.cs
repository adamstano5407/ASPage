using APIKros.Data;
using APIKros.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Seeders;

public static class DatabaseSeeder
{
    private const string Locale = "sk";
    public static async Task SeedAsync(AppDbContext context)
    {
        
        if (await context.Companies.AnyAsync())
            return;

        var companies = new Faker<Company>(Locale)
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Code, f => f.Random.AlphaNumeric(6).ToUpper())
            .Generate(3);

        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();

        foreach (var company in companies)
        {
            var employees = new Faker<Employee>(Locale)
                .RuleFor(e => e.EmployeeNumber, f => $"EMP-{f.UniqueIndex:D5}")
                .RuleFor(e => e.Title, f => f.Name.Prefix())
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.CompanyId, company.Id)
                .Generate(10);

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            company.ManagerId = employees[0].Id;

            var divisions = new Faker<Division>(Locale)
                .RuleFor(d => d.Name, f => f.Commerce.Department())
                .RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper())
                .RuleFor(d => d.CompanyId, company.Id)
                .RuleFor(d => d.ManagerId, f => f.PickRandom(employees).Id)
                .Generate(3);

            context.Divisions.AddRange(divisions);
            await context.SaveChangesAsync();

            foreach (var division in divisions)
            {
                var projects = new Faker<Project>(Locale)
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(5).ToUpper())
                    .RuleFor(p => p.DivisionId, division.Id)
                    .RuleFor(p => p.ManagerId, f => f.PickRandom(employees).Id)
                    .Generate(3);

                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();

                foreach (var project in projects)
                {
                    var departments = new Faker<Department>(Locale)
                        .RuleFor(d => d.Name, f => f.Commerce.Department())
                        .RuleFor(d => d.Code, f => f.Random.AlphaNumeric(5).ToUpper())
                        .RuleFor(d => d.ProjectId, project.Id)
                        .RuleFor(d => d.ManagerId, f => f.PickRandom(employees).Id)
                        .Generate(3);

                    context.Departments.AddRange(departments);
                }

                await context.SaveChangesAsync();
            }
        }

        await context.SaveChangesAsync();
    }
}