using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class ProcessStep : BaseEntity
    {
        public string StepName { get; set; } = string.Empty;
        public string? PerformedBy { get; set; }
        public DateTime? PerformedAt { get; set; }
        public string? Action { get; set; }
        public ProcessStepStatus Status { get; set; } = ProcessStepStatus.Pending;
        public string? ValidationResult { get; set; }
        public string? Comments { get; set; }
        public DateTime? CompletedAt { get; set; }


        [ForeignKey(nameof(Process))]
        public Guid ProcessId { get; set; }
        [JsonIgnore]
        public virtual Process Process { get; set; } = null!;


        [ForeignKey(nameof(WorkflowStep))]
        public Guid WorkflowStepId { get; set; }
        [JsonIgnore]
        public virtual WorkflowStep WorkflowStep { get; set; } = null!;
    }
}
