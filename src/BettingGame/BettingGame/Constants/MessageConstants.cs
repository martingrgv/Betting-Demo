namespace BettingGame.Constants;

public static class MessageConstants
{
    // Game
    public const string EnterInputMessage = "Please, submit action:";
    public const string ExitMessage = "Thank you for playing! Hope to see you again soon.";
    public const string WinMessage = "Congrats - you won ${0}!";
    public const string LoseMessage = "No luck this time!";
    public const string BalanceReviewMessage = "Your current balance is: ${0}";
    public const string InvalidCommandUsageMessage = "Invalid usage! Usage: deposit/withdraw/bet <amount>";
    public const string NoCommandMessage = "No choice was made";
    public const string BetErrorDueToInsufficientBalanceMessage = "You cannot place your bet due to insufficient balance";
    public const string CommonErrorMessage = "Error occured! Please, contact game support";
    
    // Wallet
    public const string SuccessDepositMessage = "Your deposit of ${0} was successful.";
    public const string SuccessWithdrawMessage = "Your withdraw of ${0} was successful.";
    public const string InsufficientBalanceWithdrawMessage = "Cannot withdraw due to insufficient balance!";
    
    
}