using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace WorkflowTrackingSystem.Infrastructure.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "WorkflowTrackingSystem.Api"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<AppDbContextFactory>()
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"ConnectionString: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString,
                b => b.MigrationsAssembly("WorkflowTrackingSystem.Infrastructure")
                      .EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}