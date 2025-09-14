using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WorkflowTrackingSystem.Application.DTOs;
using WorkflowTrackingSystem.Domain.Entities;

namespace WorkflowTrackingSystem.Api.Requests.Workflow
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