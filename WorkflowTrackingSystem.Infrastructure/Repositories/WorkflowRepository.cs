using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Contexts;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Infrastructure.Repositories
{
    public class WorkflowRepository : BaseRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Workflows.AnyAsync(w => w.Id == id);
        }

        public async Task<PaginatedList<Workflow>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Workflows.Include(w => w.Steps).AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<Workflow>(items, totalCount, pageNumber, pageSize);
        }
    }
}
