using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Application.Services.Interfaces
{
    public interface IWorkflowService
    {
        Task<IEnumerable<Workflow>> GetAllWorkflowsAsync();
        Task<Workflow?> GetWorkflowByIdAsync(Guid id);
        Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto dto);
        Task<Workflow> UpdateWorkflowAsync(Guid id, CreateWorkflowDto dto);
        Task DeleteWorkflowAsync(Guid id);
    }
}
