namespace WorkflowTrackingSystem.Application.DTOs
{
    public class WorkflowDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<WorkflowStepDto> Steps { get; set; } = new();
    }
}
