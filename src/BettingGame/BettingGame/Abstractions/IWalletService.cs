namespace BettingGame.Abstractions;

public interface IWalletService
{
    Task<Wallet?> GetById(Guid id);
    Task<Wallet?> GetByPlayerId(Guid playerId);
    Task<Wallet> CreateWalletAsync(Guid playerId);
    Task DepositAsync(Wallet wallet, decimal amount);
    Task<decimal> WithdrawAsync(Wallet wallet, decimal amount);
}