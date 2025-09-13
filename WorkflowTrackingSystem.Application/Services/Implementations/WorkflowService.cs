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

        public async Task<BaseResponse<PaginatedList<WorkflowDto>>> GetAllWorkflowsAsync(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetch all workflows with pagination. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

                var pagedResult = await _workflowRepository.GetPagedAsync(pageNumber, pageSize);
                var workflowsDto = _mapper.Map<IEnumerable<WorkflowDto>>(pagedResult.Items);
                var paginatedList = new PaginatedList<WorkflowDto>(workflowsDto, pagedResult.TotalCount, pageNumber, pageSize);
                return BaseResponse<PaginatedList<WorkflowDto>>.Success(paginatedList, "Workflows retrieved successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while fetching all workflows.");
                return BaseResponse<PaginatedList<WorkflowDto>>.Fail("An error occurred while fetching all workflows.",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<WorkflowDto>> GetWorkflowByIdAsync(Guid id)
        {
            LogInformation("Fetching workflow with ID: {Id}", id);
            try
            {
                //var workflow = await _workflowRepository.FindAsync(x => x.Id == id, includes: new[] { "Steps", "Processes" });
                var workflow = await _workflowRepository.FindAsync(x => x.Id == id, includes: new[] { "Steps" });
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
                var existingWorkflow = await _workflowRepository.FindAsync(x => x.Id == id, includes: new[] { "Steps" });
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
                // Manually map properties except for Id
                existingWorkflow.Name = workflowDto.Name;
                existingWorkflow.Description = workflowDto.Description;

                if (workflowDto.Steps != null)
                {
                    if (workflowDto.Steps.All(s => s.Id == Guid.Empty))
                    {
                        existingWorkflow.Steps.Clear();
                        foreach (var dtoStep in workflowDto.Steps)
                        {
                            var newStep = _mapper.Map<WorkflowStep>(dtoStep);
                            newStep.WorkflowId = existingWorkflow.Id;
                            existingWorkflow.Steps.Add(newStep);
                        }
                    }
                    else
                    {
                        var dtoStepIds = workflowDto.Steps.Select(s => s.Id).ToList();
                        var stepsToRemove = existingWorkflow.Steps.Where(s => !dtoStepIds.Contains(s.Id)).ToList();
                        foreach (var step in stepsToRemove)
                        {
                            existingWorkflow.Steps.Remove(step);
                        }
                        foreach (var dtoStep in workflowDto.Steps)
                        {
                            var entityStep = existingWorkflow.Steps.FirstOrDefault(s => s.Id == dtoStep.Id);
                            if (entityStep != null)
                            {
                                entityStep.StepName = dtoStep.StepName;
                                entityStep.AssignedTo = dtoStep.AssignedTo;
                                entityStep.ActionType = dtoStep.ActionType;
                                entityStep.NextStep = dtoStep.NextStep;
                            }
                            else
                            {
                                var newStep = _mapper.Map<WorkflowStep>(dtoStep);
                                newStep.WorkflowId = existingWorkflow.Id;
                                existingWorkflow.Steps.Add(newStep);
                            }
                        }
                    }
                }
                existingWorkflow.UpdatedAt = DateTime.UtcNow;
                await _workflowRepository.UpdateAsync(existingWorkflow);
                var updatedDto = _mapper.Map<WorkflowDto>(existingWorkflow);
                return BaseResponse<WorkflowDto>.Success(updatedDto, "Workflow updated successfully");
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
