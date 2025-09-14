using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Domain.Repositories
{
    public interface IProcessRepository : IBaseRepository<Process>
    {
        Task<PaginatedList<Process>> GetPagedAsync(int pageNumber, int pageSize, Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null);
    }
}
