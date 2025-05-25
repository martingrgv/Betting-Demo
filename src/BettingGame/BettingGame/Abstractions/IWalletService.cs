using BettingGame.Data.Models;

namespace BettingGame.Abstractions;

public interface IWalletService
{
    Task<Wallet?> GetByPlayerIdAsync(Guid playerId);
    Task<Wallet> CreateWalletAsync(Guid playerId);
    Task DepositAsync(Wallet wallet, decimal amount);
    Task<decimal> WithdrawAsync(Wallet wallet, decimal amount);
}