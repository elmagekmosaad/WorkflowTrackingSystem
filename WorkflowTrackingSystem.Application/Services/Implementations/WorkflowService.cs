using AutoMapper;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Repositories;
using Microsoft.Extensions.Logging;
using WorkflowTrackingSystem.Shared;


namespace WorkflowTrackingSystem.Application.Services.Implementations
{
    public class WorkflowService : BaseService, IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkflowService> _logger;

        public WorkflowService(IWorkflowRepository workflowRepository, IMapper mapper, ILogger<WorkflowService> logger)
        {
            _workflowRepository = workflowRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<IEnumerable<WorkflowDto>>> GetAllWorkflowsAsync()
        {
            try
            {
                _logger.LogInformation("Fetch all workflows.");

                var workflows = await _workflowRepository.GetAllAsync();
                var workflowsDto = _mapper.Map<IEnumerable<WorkflowDto>>(workflows);
                return BaseResponse<IEnumerable<WorkflowDto>>.Success(workflowsDto, "Workflows retrieved successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while fetching all workflows.");
                return BaseResponse<IEnumerable<WorkflowDto>>.Fail("An error occurred while fetching all workflows.",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<WorkflowDto>> GetWorkflowByIdAsync(Guid id)
        {
            LogInformation("Fetching workflow with ID: {Id}", id);
            try
            {
                var workflow = await _workflowRepository.FindAsync(x => x.Id == id, includes: new[] { "Steps", "Processes" });
                if (workflow == null)
                {
                    LogWarning("Workflow not found for ID: {Id}", id);
                    return BaseResponse<WorkflowDto>.Fail("Workflow not found", new List<string> { "Workflow not found" });
                }
                var workflowDto = _mapper.Map<WorkflowDto>(workflow);
                return BaseResponse<WorkflowDto>.Success(workflowDto, "Workflow retrieved successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while fetching workflow with ID: {Id}", id);
                return BaseResponse<WorkflowDto>.Fail("An error occurred while fetching workflow",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<WorkflowDto>> CreateWorkflowAsync(WorkflowDto workflowCreateDto)
        {
            LogInformation("Adding new workflow.");
            try
            {
                var workflow = _mapper.Map<Workflow>(workflowCreateDto);
                //check if Steps mapped automaticly or not otherwise add them manually

                await _workflowRepository.AddAsync(workflow);
                return BaseResponse<WorkflowDto>.Success(workflowCreateDto, "Workflow added successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while adding workflow.");
                return BaseResponse<WorkflowDto>.Fail("An error occurred while adding workflow",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<WorkflowDto>> UpdateWorkflowAsync(Guid id, WorkflowDto workflowDto)
        {
            LogInformation("Updating workflow with ID: {Id}", workflowDto.Id);

            try
            {
                var existingWorkflow = await _workflowRepository.GetByIdAsync(workflowDto.Id);
                if (existingWorkflow == null)
                {
                    LogWarning("Workflow not found for ID: {Id}", workflowDto.Id);
                    return BaseResponse<WorkflowDto>.Fail("Workflow not found", new List<string> { "Workflow not found" });
                }
                if (existingWorkflow.Id != id)
                {
                    LogWarning("The provided ID does not match the existing workflow ID: {Id}", workflowDto.Id);
                    return BaseResponse<WorkflowDto>.Fail("Workflow ID mismatch", new List<string> { "The provided workflow ID does not match any existing record" });
                }
                //check if Steps mapped automaticly or not otherwise add them manually

                _mapper.Map(workflowDto, existingWorkflow);

                existingWorkflow.UpdatedAt = DateTime.UtcNow;

                await _workflowRepository.UpdateAsync(existingWorkflow);
                return BaseResponse<WorkflowDto>.Success(workflowDto, "Workflow updated successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while updating workflow with ID: {Id}", workflowDto.Id);
                return BaseResponse<WorkflowDto>.Fail("An error occurred while updating workflow",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<string>> DeleteWorkflowAsync(Guid id)
        {
            LogInformation("Deleting workflow with ID: {Id}", id);
            try
            {
                var workflow = await _workflowRepository.GetByIdAsync(id);
                if (workflow == null)
                {
                    LogWarning("Workflow not found for ID: {Id}", id);
                    return BaseResponse<string>.Fail("Workflow not found", new List<string> { "Workflow not found" });
                }

                await _workflowRepository.DeleteAsync(workflow.Id);
                return BaseResponse<string>.Success(null, "Workflow deleted successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while deleting workflow with ID: {Id}", id);
                return BaseResponse<string>.Fail("An error occurred while deleting workflow",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

    }
}
