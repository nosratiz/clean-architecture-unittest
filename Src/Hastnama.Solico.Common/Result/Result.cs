using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Common.Result
{
    public class Result<T>
    {
        public T Data { get; private set; }
        public string Message { get; set; }

        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result<T> SuccessFul(T data) => new() { ApiResult = new OkObjectResult(data), Data = data, Message = null, Success = true };

        public static Result<T> SuccessFul(ObjectResult success) => new() { ApiResult = success, Success = true };

        public static Result<T> Failed(ObjectResult error) => new() { ApiResult = error, Success = false, Message = error.Value?.ToString() };

        public static Result<T> Failed(string message) => new() {Message = message, Success = false};
    }

    public class Result
    {
        public string Message { get; set; }

        public ObjectResult ApiResult { get; set; }
        public bool Success { get; set; }

        public static Result SuccessFul() => new() { Message = null, Success = true };
        
        public static Result SuccessFul(ObjectResult success) => new Result { ApiResult = success, Success = true };


        public static Result Failed(ObjectResult error) => new() { ApiResult = error, Success = false, Message = error.Value?.ToString() };

        public static Result Failed(string message) => new() {Message = message, Success = false};  
    }
}