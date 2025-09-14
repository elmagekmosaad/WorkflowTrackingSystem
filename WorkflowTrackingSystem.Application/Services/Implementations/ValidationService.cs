using System.Threading.Tasks;

namespace WorkflowTrackingSystem.Application.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        public async Task<(bool IsValid, string? Message)> ValidateStepAsync(string stepName, string action)
        {
            await Task.Delay(100); // Simulate external validation
            // Example: Always valid except for step "Finance Approval" with action "reject"
            if (stepName == "Finance Approval" && action.ToLower() == "reject")
                return (false, "Finance approval rejected by external validation.");
            return (true, null);
        }
    }
}
