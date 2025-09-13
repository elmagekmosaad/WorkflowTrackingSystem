using System.ComponentModel.DataAnnotations;
using WorkflowTrackingSystem.Application.DTOs;

namespace WorkflowTrackingSystem.Api.Requests
{
    public class CreateWorkflowRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public List<CreateWorkflowStepRequest> Steps { get; set; } = new();
    }
}