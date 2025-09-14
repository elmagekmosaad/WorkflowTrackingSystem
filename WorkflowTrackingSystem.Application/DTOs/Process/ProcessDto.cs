
using WorkflowTrackingSystem.Domain.Enums;

namespace WorkflowTrackingSystem.Application.DTOs.Process
{
    public class ProcessDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Initiator { get; set; } = string.Empty;
        public ProcessStatus Status { get; set; } = ProcessStatus.Pending;

    }
}
