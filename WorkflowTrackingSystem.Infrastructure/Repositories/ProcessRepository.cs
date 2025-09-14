using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Contexts;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Infrastructure.Repositories
{
    public class ProcessRepository : BaseRepository<Process>, IProcessRepository
    {
        public ProcessRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Process>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    Guid? workflowId = null,
    ProcessStatus? status = null,
    string? assignedTo = null)
        {
            var query = _context.Processes.AsNoTracking().AsQueryable();

            if (workflowId.HasValue)
                query = query.Where(p => p.WorkflowId == workflowId.Value);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (!string.IsNullOrEmpty(assignedTo))
                query = query.Where(p => p.Initiator == assignedTo);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.ProcessSteps)
                .ToListAsync();

            return new PaginatedList<Process>(items, totalCount, pageNumber, pageSize);
        }

    }
}
