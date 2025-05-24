namespace BettingGame.Common;

public record Result(bool IsSuccess, string? Message, Exception? Exception)
{
    public static Result Success(string message) => new(true, message, null); 
    public static Result Failure(string message, Exception exception) => new(false, message, exception); 
    public static Result Failure(Exception exception) => new(false, default, exception); 
}