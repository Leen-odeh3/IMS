namespace IMS.Application.Common;
public class Result<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }

    private Result(bool success, T? data, string message)
    {
        Success = success;
        Data = data;
        Message = message;
    }

    public static Result<T> Ok(T data)
    {
        return new Result<T>(true, data, string.Empty);
    }

    public static Result<T> Fail(string message)
    {
        return new Result<T>(false, default, message);
    }
}