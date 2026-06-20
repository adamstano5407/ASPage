using APIKros.Data;
using APIKros.Seeders;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using APIKros.Validators;
using APIKros.Validators.Employee;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeRequestValidator>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var connectionString =
    $"Server={Environment.GetEnvironmentVariable("DB_HOST")},{Environment.GetEnvironmentVariable("DB_PORT")};" +
    $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
    $"User Id={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
    $"TrustServerCertificate={Environment.GetEnvironmentVariable("DB_TRUST_CERTIFICATE")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await DatabaseSeeder.SeedAsync(context);

    return;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



