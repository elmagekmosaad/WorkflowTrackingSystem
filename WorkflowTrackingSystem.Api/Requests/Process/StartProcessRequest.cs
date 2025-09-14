using System.ComponentModel.DataAnnotations;

namespace WorkflowTrackingSystem.Api.Requests.Process
{
    public class StartProcessRequest
    {
        [Required]
        public Guid WorkflowId { get; set; }
        [Required]
        public string Initiator { get; set; } = string.Empty;
    }
}