using APIKros.Data;
using APIKros.Extensions;
using APIKros.Handlers;
using APIKros.Seeders;
using ASPage.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

//Rate limiter 
builder.Services.AddRateLimiterCustom();

builder.Services.AddOpenApiWithServerUrl(builder.Environment);
// Add services to the container.
builder.Services.AddRepositories();
builder.Services.AddServices();

//fluent validation
builder.Services.AddValidation();
builder.Services.AddMappings();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSqlServerConnectionString()));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapRoutes();
app.UseExceptionHandler();

app.Run();



