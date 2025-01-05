using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Registers controllers for API endpoints
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint discovery

// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "QC Fetch API",
        Version = "v1",
        Description = "An API to fetch and process QC course data.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "David Rodriguez",
            Email = "D.Rodriguez.1865@gmail.com",
            Url = new Uri("https://github.com/DavidRod1865"),
        }
    });
});

// Register the HttpClient service
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging()) // Swagger for development and staging environments
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QC Courses API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root (optional)
    });
}

app.UseHttpsRedirection(); // Redirects HTTP to HTTPS

app.UseAuthorization(); // Adds authorization middleware (if needed)

app.MapControllers(); // Maps controllers to routes

app.Run();
