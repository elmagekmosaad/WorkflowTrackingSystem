using Microsoft.Extensions.Logging;

namespace WorkflowTrackingSystem.Shared
{
    public abstract class BaseService
    {
        protected readonly ILogger Logger;

        protected BaseService()
        {
            Logger = new LoggerFactory().CreateLogger<BaseService>();
        }

        protected BaseResponse<T> SuccessResponse<T>(string message, T data) where T : class
        {
            return BaseResponse<T>.Success(data, message);
        }

        protected BaseResponse<T> ErrorResponse<T>(string message, IEnumerable<string> errors) where T : class
        {
            return BaseResponse<T>.Fail(message, errors.ToList());
        }
      
        protected void LogWarning(string message, params object[] args)
        {
            Logger.LogWarning(message, args);
        }

        protected void LogError(Exception ex, string message, params object[] args)
        {
            Logger.LogError(ex, message, args);
        }

        protected void LogInformation(string message, params object[] args)
        {
            Logger.LogInformation(message, args);
        }
    }
}
