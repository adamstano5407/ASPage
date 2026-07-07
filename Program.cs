using APIKros.Data;
using APIKros.Extensions;
using APIKros.Seeders;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;


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
builder.Services.AddRepositories();
builder.Services.AddServices();

//fluent validation
builder.Services.AddValidation();


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

app.MapRoutes();
// CORS can be configured later when a frontend application is added.

if (app.Environment.IsDevelopment())
{
    app.MapPost("/dev/seed", async (AppDbContext context) =>
    {
        await DatabaseSeeder.SeedAsync(context);
        return Results.Ok("Database seeded.");
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var controllers = app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    app.UseRateLimiter();
    controllers.RequireRateLimiting("default");
}
    
app.Run();



