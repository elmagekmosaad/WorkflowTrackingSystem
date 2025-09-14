using Microsoft.EntityFrameworkCore;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Contexts;
using WorkflowTrackingSystem.Infrastructure.Repositories;
using WorkflowTrackingSystem.Shared;

namespace ProcessTrackingSystem.Infrastructure.Repositories
{
    public class ProcessRepository : BaseRepository<Process>, IProcessRepository
    {
        public ProcessRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Process>> GetPagedAsync(int pageNumber, int pageSize, Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null)
        {
            var query = _context.Processes.Include(w => w.ProcessSteps).AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<Process>(items, totalCount, pageNumber, pageSize);
        }
    }
}
