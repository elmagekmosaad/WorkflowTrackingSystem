namespace WorkflowTrackingSystem.Application.Services.Interfaces
{
    public interface IValidationService
    {
        Task<(bool IsValid, string? Message)> ValidateStepAsync(string stepName, string action);
    }
}
