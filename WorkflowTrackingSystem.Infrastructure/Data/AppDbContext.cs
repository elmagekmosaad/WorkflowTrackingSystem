using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Infrastructure.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Workflow configuration
            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
            });
            #endregion

            #region WorkflowStep configuration
            modelBuilder.Entity<WorkflowStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Workflow)
                      .WithMany(w => w.Steps)
                      .HasForeignKey(e => e.WorkflowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion


        }
    }
}
