using AutoMapper;
using Microsoft.Extensions.Logging;
using WorkflowTrackingSystem.Application.DTOs.Process;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Enums;
using WorkflowTrackingSystem.Domain.Repositories;
using WorkflowTrackingSystem.Shared;


namespace WorkflowTrackingSystem.Application.Services.Implementations
{
    public class ProcessService : BaseService, IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProcessService> _logger;
        private readonly IWorkflowRepository _workflowRepository;

        public ProcessService(IProcessRepository processRepository, IMapper mapper, ILogger<ProcessService> logger, IWorkflowRepository workflowRepository)
        {
            _processRepository = processRepository;
            _mapper = mapper;
            _logger = logger;
            _workflowRepository = workflowRepository;
        }

        public async Task<BaseResponse<ProcessDto>> StartProcessAsync(ProcessDto processDto)
        {
            _logger.LogInformation("Starting a new process for WorkflowId: {WorkflowId} by Initiator: {Initiator}", processDto.WorkflowId, processDto.Initiator);

            var workflow = await _workflowRepository.FindAsync(x => x.Id == processDto.WorkflowId, includes: new[] { "Steps" });
            if (workflow == null)
            {
                _logger.LogWarning("Workflow not found for WorkflowId: {WorkflowId}", processDto.WorkflowId);
                return BaseResponse<ProcessDto>.Fail("Workflow not found", new List<string> { "Workflow not found" });
            }
            try
            {
                var firstStep = workflow.Steps.OrderBy(s => s.Order).FirstOrDefault();
                if (firstStep == null)
                {
                    _logger.LogWarning("No steps defined in the workflow for WorkflowId: {WorkflowId}", processDto.WorkflowId);
                    return BaseResponse<ProcessDto>.Fail("No steps defined in the workflow", new List<string> { "Workflow has no steps defined" });
                }
                var process = new Process
                {
                    WorkflowId = processDto.WorkflowId,
                    Initiator = processDto.Initiator,
                    CurrentStep = firstStep?.StepName,
                    Status = ProcessStatus.Active
                };
                if (firstStep != null)
                {
                    process.ProcessSteps.Add(new ProcessStep
                    {
                        ProcessId = process.Id,
                        WorkflowStepId = firstStep.Id,
                        StepName = firstStep.StepName,
                        Status = ProcessStepStatus.Pending,
                    });
                }
                var createdProcess = await _processRepository.AddAsync(process);
                var createdProcessDto = _mapper.Map<ProcessDto>(createdProcess);
                _logger.LogInformation("Process started successfully with ID: {ProcessId}", createdProcess.Id);
                return BaseResponse<ProcessDto>.Success(createdProcessDto, "Process started successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while starting process for WorkflowId: {WorkflowId}", processDto.WorkflowId);
                return BaseResponse<ProcessDto>.Fail("An error occurred while starting process",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }


        }
        public async Task<BaseResponse<ProcessDto>> ExecuteStepAsync(ExecuteStepDto executeStepDto)
        {
            _logger.LogInformation("Executing step '{StepName}' for ProcessId: {ProcessId} by PerformedBy: {PerformedBy}", executeStepDto.StepName, executeStepDto.ProcessId, executeStepDto.PerformedBy);
            try
            {
                var process = await _processRepository.FindAsync(x => x.Id == executeStepDto.ProcessId, includes: new[] { "ProcessSteps", "Workflow.Steps" });
                if (process == null)
                {
                    _logger.LogWarning("Process not found for ProcessId: {ProcessId}", executeStepDto.ProcessId);
                    return BaseResponse<ProcessDto>.Fail("Process not found", new List<string> { "Process not found" });
                }
                var currentStep = process.ProcessSteps
                    .FirstOrDefault(ps => ps.StepName == executeStepDto.StepName && ps.Status == ProcessStepStatus.Pending);
                if (currentStep == null)
                {
                    _logger.LogWarning("Current step '{StepName}' not found or already completed for ProcessId: {ProcessId}", executeStepDto.StepName, executeStepDto.ProcessId);
                    return BaseResponse<ProcessDto>.Fail("Current step not found or already completed", new List<string> { "Current step not found or already completed" });
                }

                var workflowStep = process.Workflow.Steps.FirstOrDefault(ws => ws.StepName == currentStep.StepName && ws.Order == process.Workflow.Steps.OrderBy(s => s.Order).FirstOrDefault(s => s.StepName == currentStep.StepName)?.Order);
                if (workflowStep == null)
                {
                    _logger.LogWarning("Workflow step definition not found for step '{StepName}'", currentStep.StepName);
                    return BaseResponse<ProcessDto>.Fail("Workflow step definition not found", new List<string> { "Workflow step definition not found" });
                }
                if (!string.Equals(workflowStep.AssignedTo, executeStepDto.PerformedBy, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("User '{PerformedBy}' is not allowed to perform step '{StepName}'", executeStepDto.PerformedBy, executeStepDto.StepName);
                    return BaseResponse<ProcessDto>.Fail("User is not allowed to perform this step", new List<string> { "User is not allowed to perform this step" });
                }
                if (!string.Equals(workflowStep.ActionType, executeStepDto.Action, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Action '{Action}' does not match required action type '{ActionType}' for step '{StepName}'", executeStepDto.Action, workflowStep.ActionType, executeStepDto.StepName);
                    return BaseResponse<ProcessDto>.Fail("Action does not match required action type", new List<string> { "Action does not match required action type" });
                }
                if (string.Equals(workflowStep.StepName, "Finance Approval", StringComparison.OrdinalIgnoreCase))
                {
                    // Simulate external API validation
                    bool isValid = await SimulateFinanceApiValidationAsync(process, currentStep);
                    if (!isValid)
                    {
                        _logger.LogWarning("Finance validation failed for process {ProcessId}", process.Id);
                        return BaseResponse<ProcessDto>.Fail("Finance validation failed", new List<string> { "Finance validation failed" });
                    }
                }
                currentStep.Status = executeStepDto.Action.ToLower() == "reject" ? ProcessStepStatus.Rejected : ProcessStepStatus.Completed;
                currentStep.PerformedBy = executeStepDto.PerformedBy;
                currentStep.PerformedAt = DateTime.UtcNow;
                currentStep.Action = executeStepDto.Action;
                currentStep.Comments = executeStepDto.Comments;


                if (executeStepDto.Action.ToLower() == "reject")
                {
                    process.Status = ProcessStatus.Rejected;
                }
                else
                {
                    var nextStepName = currentStep.WorkflowStep.NextStep;
                    if (nextStepName == "Completed" || string.IsNullOrEmpty(nextStepName))
                    {
                        process.CurrentStep = "Completed";
                        process.Status = ProcessStatus.Completed;
                    }
                    else
                    {
                        var nextWorkflowStep = process.Workflow.Steps
                   .Where(s => s.Order > workflowStep.Order)
                   .OrderBy(s => s.Order)
                   .FirstOrDefault();
                        if (nextWorkflowStep != null)
                        {
                            process.CurrentStep = nextWorkflowStep.StepName;
                            process.ProcessSteps.Add(new ProcessStep
                            {
                                ProcessId = process.Id,
                                WorkflowStepId = nextWorkflowStep.Id,
                                StepName = nextWorkflowStep.StepName,
                                Status = ProcessStepStatus.Pending,
                            });
                        }

                    }
                }

                process.UpdatedAt = DateTime.UtcNow;
                await _processRepository.UpdateAsync(process);
                var processDto = _mapper.Map<ProcessDto>(process);
                _logger.LogInformation("Step '{StepName}' executed successfully for ProcessId: {ProcessId}", executeStepDto.StepName, executeStepDto.ProcessId);
                return BaseResponse<ProcessDto>.Success(processDto, "Step executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing step '{StepName}' for ProcessId: {ProcessId}", executeStepDto.StepName, executeStepDto.ProcessId);
                return BaseResponse<ProcessDto>.Fail("An error occurred while executing step",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }


        public async Task<BaseResponse<PaginatedList<ProcessDto>>> GetAllProcesssAsync(int pageNumber, int pageSize, Guid? workflowId = null, ProcessStatus? status = null, string? assignedTo = null)
        {
            try
            {
                _logger.LogInformation("Fetch all processs with pagination. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

                var pagedResult = await _processRepository.GetPagedAsync(pageNumber, pageSize, workflowId, status, assignedTo);
                var processsDto = _mapper.Map<IEnumerable<ProcessDto>>(pagedResult.Items);
                var paginatedList = new PaginatedList<ProcessDto>(processsDto, pagedResult.TotalCount, pageNumber, pageSize);
                return BaseResponse<PaginatedList<ProcessDto>>.Success(paginatedList, "Processs retrieved successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while fetching all processs.");
                return BaseResponse<PaginatedList<ProcessDto>>.Fail("An error occurred while fetching all processs.",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<BaseResponse<ProcessDto>> GetProcessByIdAsync(Guid id)
        {
            LogInformation("Fetching process with ID: {Id}", id);
            try
            {
                var process = await _processRepository.FindAsync(x => x.Id == id, includes: new[] { "ProcessSteps", "Workflow" });
                if (process == null)
                {
                    LogWarning("Process not found for ID: {Id}", id);
                    return BaseResponse<ProcessDto>.Fail("Process not found", new List<string> { "Process not found" });
                }
                var processDto = _mapper.Map<ProcessDto>(process);
                return BaseResponse<ProcessDto>.Success(processDto, "Process retrieved successfully");
            }
            catch (Exception ex)
            {
                LogError(ex, "An error occurred while fetching process with ID: {Id}", id);
                return BaseResponse<ProcessDto>.Fail("An error occurred while fetching process",
                    new List<string> { ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        private async Task<bool> SimulateFinanceApiValidationAsync(Process process, ProcessStep step)
        {
            await Task.Delay(100); // Simulate network delay
            // Always return true for simulation, you can add logic here
            return true;
        }
    }
}
