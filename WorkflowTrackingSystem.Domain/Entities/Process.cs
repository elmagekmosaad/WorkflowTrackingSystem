using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class Process : BaseEntity
    {

        [ForeignKey(nameof(Workflow))]
        public Guid WorkflowId { get; set; }
        [JsonIgnore]
        public virtual Workflow Workflow { get; set; } = null!;

        [Required]
        [MaxLength(250)]
        public string Initiator { get; set; } = string.Empty;

        [Required]
        public ProcessStatus Status { get; set; } = ProcessStatus.Active;
        [Required]
        [MaxLength(250)]
        public string CurrentStep { get; set; } = string.Empty;
        public virtual ICollection<ProcessStep> ProcessSteps { get; set; } = new List<ProcessStep>();


    }
}
