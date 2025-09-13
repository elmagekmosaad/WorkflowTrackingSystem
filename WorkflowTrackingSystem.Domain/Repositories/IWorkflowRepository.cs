using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Domain.Repositories
{
    public interface IWorkflowRepository : IBaseRepository<Workflow>
    {
        Task<bool> ExistsAsync(Guid id);
    }
}
