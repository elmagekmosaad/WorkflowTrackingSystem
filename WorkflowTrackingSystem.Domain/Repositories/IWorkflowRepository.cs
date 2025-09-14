using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Domain.Repositories
{
    public interface IWorkflowRepository : IBaseRepository<Workflow>
    {
        Task<bool> ExistsAsync(Guid id);
        Task<PaginatedList<Workflow>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
