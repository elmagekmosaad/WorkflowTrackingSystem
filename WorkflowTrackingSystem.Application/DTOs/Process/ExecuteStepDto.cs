using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Application.DTOs.Process
{
    public class ExecuteStepDto
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
