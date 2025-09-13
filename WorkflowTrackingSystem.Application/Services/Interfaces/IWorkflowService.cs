using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Application.Services.Interfaces
{
    public interface IWorkflowService
    {
        Task<BaseResponse<IEnumerable<WorkflowDto>>> GetAllWorkflowsAsync();
        Task<BaseResponse<WorkflowDto>> GetWorkflowByIdAsync(Guid id);
        Task<BaseResponse<WorkflowDto>> CreateWorkflowAsync(WorkflowDto workflowDto);
        Task<BaseResponse<WorkflowDto>> UpdateWorkflowAsync(Guid id, WorkflowDto workflowDto);
        Task<BaseResponse<string>> DeleteWorkflowAsync(Guid id);
    }
}
