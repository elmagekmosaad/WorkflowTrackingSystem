using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Application.Services.Interfaces;
using WorkflowTrackingSystem.Domain.Entities;
using WorkflowTrackingSystem.Domain.Repositories;

namespace WorkflowTrackingSystem.Application.Services.Implementations
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRepository _workflowRepository;

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            _workflowRepository = workflowRepository;
        }

        public async Task<IEnumerable<Workflow>> GetAllWorkflowsAsync()
        {
            return await _workflowRepository.GetAllAsync();
        }

        public async Task<Workflow?> GetWorkflowByIdAsync(Guid id)
        {
            return await _workflowRepository.FindAsync(x => x.Id == id, includes: new[] { "Steps", "Processes" });
        }

        public async Task<Workflow> CreateWorkflowAsync(CreateWorkflowDto dto)
        {
            var workflow = new Workflow
            {
                Name = dto.Name,
                Description = dto.Description,
                Steps = dto.Steps.Select((step, index) => new WorkflowStep
                {
                    StepName = step.StepName,
                    AssignedTo = step.AssignedTo,
                    ActionType = step.ActionType,
                    NextStep = step.NextStep,
                    
                }).ToList()
            };

            return await _workflowRepository.AddAsync(workflow);
        }

        public async Task<Workflow> UpdateWorkflowAsync(Guid id, CreateWorkflowDto dto)
        {
            var existingWorkflow = await _workflowRepository.GetByIdAsync(id);
            if (existingWorkflow == null)
                throw new ArgumentException("Workflow not found");

            existingWorkflow.Name = dto.Name;
            existingWorkflow.Description = dto.Description;
            existingWorkflow.UpdatedAt = DateTime.UtcNow;

            return _workflowRepository.Update(existingWorkflow);
        }

        public async Task DeleteWorkflowAsync(Guid id)
        {
            var workFlow = await _workflowRepository.GetByIdAsync(id);
            if (workFlow == null)
                throw new ArgumentException("Workflow not found");

            _workflowRepository.Delete(workFlow);
        }
    }
}
