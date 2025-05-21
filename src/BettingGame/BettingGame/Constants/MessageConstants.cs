namespace BettingGame.Constants;

public static class MessageConstants
{
    // Game
    public const string EnterInputMessage = "Please, submit action:";
    public const string ExitMessage = "Thank you for playing! Hope to see you again soon.";
    public const string WinMessage = "Congrats - you won {0}! Your current balance is: {1}";
    public const string LoseMessage = "No luck this time! Your current balance is {0}";
    public const string BalanceReviewMessage = "Your current balance is {0}";
    public const string InvalidCommandUsageMessage = "Invalid usage! Usage: deposit/withdraw/bet <amount>";
    
    // Wallet
    public const string SuccessDepositMessage = "Your deposit of ${0} was successful.";
    public const string SuccessWithdrawMessage = "Your withdraw of {0} was successful.";
    public const string InsufficientBalanceWithdrawMessage = "Cannot withdraw due to insufficient balance!";
    
    
}