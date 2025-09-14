using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowTrackingSystem.Application.DTOs.Workflow;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Shared;

namespace WorkflowTrackingSystem.Application.Services.Interfaces
{
    public interface IWorkflowService
    {
        Task<BaseResponse<PaginatedList<WorkflowDto>>> GetAllWorkflowsAsync(int pageNumber, int pageSize);
        Task<BaseResponse<WorkflowDto>> GetWorkflowByIdAsync(Guid id);
        Task<BaseResponse<WorkflowDto>> CreateWorkflowAsync(WorkflowDto workflowDto);
        Task<BaseResponse<WorkflowDto>> UpdateWorkflowAsync(Guid id, WorkflowDto workflowDto);
        Task<BaseResponse<string>> DeleteWorkflowAsync(Guid id);
    }
}
