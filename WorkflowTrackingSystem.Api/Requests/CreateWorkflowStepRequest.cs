using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Api.Requests
{
    public class CreateWorkflowStepRequest
    {
        [Required]
        public string StepName { get; set; } = string.Empty;
        [Required]
        public string AssignedTo { get; set; } = string.Empty;
        [Required]
        public string ActionType { get; set; } = string.Empty;
        [Required]
        public string NextStep { get; set; } = string.Empty;
    }
}