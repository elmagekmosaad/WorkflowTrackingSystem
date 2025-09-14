using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Api.Requests.Process
{
    public class ExecuteProcessRequest
    {
        [Required]
        public Guid ProcessId { get; set; }

        [Required]
        public string StepName { get; set; } = string.Empty;

        [Required]
        public string PerformedBy { get; set; } = string.Empty;

        [Required]
        public string Action { get; set; } = string.Empty;

        public string? Comments { get; set; }
    }
}