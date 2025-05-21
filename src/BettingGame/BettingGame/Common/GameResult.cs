namespace BettingGame.Common;

public class GameResult
{
    public GameResult(bool isWin, decimal betAmount, decimal winAmount)
    {
        IsWin = isWin;
        BetAmount = betAmount;
        WinAmount = winAmount;
    }
    
    public decimal BetAmount { get; }
    public decimal WinAmount { get; }
    public bool IsWin { get; }
}