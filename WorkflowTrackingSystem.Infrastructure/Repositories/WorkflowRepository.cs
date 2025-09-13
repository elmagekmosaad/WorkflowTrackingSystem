using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Infrastructure.Data;

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
    }
}
