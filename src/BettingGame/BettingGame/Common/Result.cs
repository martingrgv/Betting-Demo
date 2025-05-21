namespace BettingGame.Common;

public class Result
{
    private Result(bool isSuccess, string? message, Exception? exception)
    {
        IsSuccess = isSuccess;
        Message = message;
        Exception = exception;
    }
    
    public bool IsSuccess { get; }
    public string Message { get; }
    public Exception Exception { get; }

    public static Result Success(string message) => new(true, message, null); 
    public static Result Failure(Exception exception) => new(false, default, exception); 
}