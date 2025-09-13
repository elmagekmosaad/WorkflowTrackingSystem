using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Application.DTOs
{
    public class CreateWorkflowStepDto
    {
        [Required]
        public string StepName { get; set; } = string.Empty;

        [Required]
        public string AssignedTo { get; set; } = string.Empty;

        [Required]
        public string ActionType { get; set; } = string.Empty;

        public string? NextStep { get; set; }

        public bool RequiresValidation { get; set; } = false;
    }
}