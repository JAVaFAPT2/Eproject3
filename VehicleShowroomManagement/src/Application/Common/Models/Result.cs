using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Application.Common.Models
{
    /// <summary>
    /// Generic result pattern for service operations
    /// </summary>
    /// <typeparam name="T">The type of data returned</typeparam>
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? Error { get; }
        public Exception? Exception { get; }

        private Result(bool isSuccess, T? data, string? error, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            Exception = exception;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, null);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, default, error);
        }

        public static Result<T> Failure(string error, Exception exception)
        {
            return new Result<T>(false, default, error, exception);
        }

        public static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, default, exception.Message, exception);
        }
    }

    /// <summary>
    /// Non-generic result for operations that don't return data
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public Exception? Exception { get; }

        private Result(bool isSuccess, string? error, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            Exception = exception;
        }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }

        public static Result Failure(string error, Exception exception)
        {
            return new Result(false, error, exception);
        }

        public static Result Failure(Exception exception)
        {
            return new Result(false, exception.Message, exception);
        }
    }
}
