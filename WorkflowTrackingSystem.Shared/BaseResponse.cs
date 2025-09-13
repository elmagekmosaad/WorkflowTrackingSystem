namespace WorkflowTrackingSystem.Shared
{
    public class BaseResponse : BaseResponse<object>
    {
        public static BaseResponse Success(string message = "") =>
            new BaseResponse { Succeeded = true, Message = message };

        public static BaseResponse Fail(string message, IEnumerable<string> errors) =>
            new BaseResponse { Succeeded = false, Message = message, Errors = errors.ToList() };
    }
    public class BaseResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }

        public static BaseResponse<T> Success(T data, string message = "") =>
            new BaseResponse<T> { Succeeded = true, Message = message, Data = data };

        public static BaseResponse<T> Fail(string message, IEnumerable<string>? errors = null) =>
            new BaseResponse<T> { Succeeded = false, Message = message, Errors = errors?.ToList() ?? new List<string>() };

    }




}
