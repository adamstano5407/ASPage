using APIKros.Data;
using APIKros.Seeders;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using APIKros.Validators;
using APIKros.Validators.Employee;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;


var builder = WebApplication.CreateBuilder(args);

//Rate limiter 
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", limiter =>
    {
        limiter.Window = TimeSpan.FromHours(1);
        limiter.PermitLimit = 100;
    });
});

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


// CORS can be configured later when a frontend application is added.

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

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers()
    .RequireRateLimiting("default");

app.Run();



