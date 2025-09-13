namespace WorkflowTrackingSystem.Application.DTOs
{
    public class WorkflowStepDto
    {
        public Guid Id { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string NextStep { get; set; } = string.Empty;
    }
}
