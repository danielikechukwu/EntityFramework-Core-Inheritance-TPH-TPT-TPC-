using EF_DatabaseInheritance.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("EF_FluentAPIInheritanceConfigurationConnection");

builder.Services.AddDbContext<EF_DatabaseInheritanceDbContext>(options =>
    options.UseSqlServer(connectionString));    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("REDACTED_PROJECT_NAME API Reference");
        opt.WithTheme(ScalarTheme.DeepSpace);
    });
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
