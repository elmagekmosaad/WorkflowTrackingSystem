using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Domain.Entities
{
    public class WorkflowStep
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(250)]
        public string StepName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AssignedTo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; } = string.Empty;

        [MaxLength(250)]
        public string NextStep { get; set; } = string.Empty;


        [ForeignKey(nameof(Workflow))]
        public Guid WorkflowId { get; set; }
        [JsonIgnore]
        public virtual Workflow Workflow { get; set; } = null!;
        public int Order { get; set; }
    }
}
