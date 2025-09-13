using Asp.Versioning;
using Microsoft.OpenApi.Models;
using WorkflowTrackingSystem.Application.Services.Implementations;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region swagger
builder.Services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo()
{
    Title = "Workflow Tracking System Api",
    Version = "v1",
    Description = "task interview - Workflow Tracking System Api" +
                              " for Nano Health Suite company",
    Contact = new OpenApiContact()
    {
        Name = "Mosaad Ghanem",
        Email = "mosaadghanem97@gmail.com",
        Url = new("https://www.linkedin.com/in/elmagekmosaad/")
    },
}));
#endregion

#region api versioning
// Configure Api versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default Api version
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume default when version is not specified
    options.ReportApiVersions = true; // Adds headers for supported versions
});
#endregion

#region dependency injections
#region services
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
#endregion
#region repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
#endregion
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
