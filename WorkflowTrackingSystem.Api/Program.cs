using Asp.Versioning;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using WorkflowTrackingSystem.Api.Mapping;
using WorkflowTrackingSystem.Application.Services.Implementations;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Contexts;
using WorkflowTrackingSystem.Infrastructure.Logging;
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
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = false;
    options.ReportApiVersions = false;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion
#region db context
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion


#region dependency injections
builder.Services.AddAutoMapper(typeof(WorkflowProfile).Assembly);

#region services
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
#endregion
#region repositories
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
#endregion
#endregion
#region logging
builder.Host.UseSerilog(LoggerConfig.ConfigureLogger);
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

#region logging
app.UseSerilogRequestLogging();
#endregion

try
{
    Console.WriteLine("Starting app.Run()...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Exception during app.Run(): " + ex);
    Log.Fatal(ex, "Application api startup failed");
    throw;
}
finally
{
    Console.WriteLine("Closing Serilog logger...");
    Log.CloseAndFlush();
}
