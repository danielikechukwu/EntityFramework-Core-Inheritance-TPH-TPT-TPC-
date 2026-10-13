using EF_DatabaseInheritance.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, CancellationToken) =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "Database Inheritance Configuration";
        document.Info.Description = "Modeling these relationships efficiently is essential for both maintainability and performance. Entity Framework Core (EF Core) provides robust inheritance mapping capabilities that allow developers to align object-oriented class hierarchies with relational database structures, preserving the benefits of OOP design while ensuring optimal database efficiency.";
        return Task.CompletedTask;
    });
});

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
        opt.WithTitle("Database Inheritance API Reference");

        opt.WithTheme(ScalarTheme.DeepSpace);
    });
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
