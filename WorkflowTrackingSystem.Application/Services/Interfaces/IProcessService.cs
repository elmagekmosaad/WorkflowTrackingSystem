using WorkflowTrackingSystem.Application.DTOs.Process;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Shared;

namespace ProcessTrackingSystem.Application.Services.Interfaces
{
    public interface IProcessService
    {
        Task<BaseResponse<PaginatedList<ProcessDto>>> GetAllProcesssAsync(int pageNumber, int pageSize, Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null);
        Task<BaseResponse<ProcessDto>> StartProcessAsync(ProcessDto processDto);
        Task<BaseResponse<ProcessDto>> ExecuteStepAsync(ProcessDto processDto);
        Task<BaseResponse<ProcessDto>> GetProcessByIdAsync(Guid id);
    }
}
