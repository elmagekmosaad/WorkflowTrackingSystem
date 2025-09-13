using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class Workflow : BaseEntity
    {

        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public virtual ICollection<WorkflowStep> Steps { get; set; } = new List<WorkflowStep>();
       
    }
}
