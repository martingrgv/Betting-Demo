using System.Diagnostics;
using BettingGame.Abstractions;
using BettingGame.Constants;
using BettingGame.Data.Persistence;
using BettingGame.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BettingGame.Services;

public class WalletService : IWalletService
{
    private BettingDbContext _dbContext;
    
    public WalletService(BettingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Wallet?> GetById(Guid id)
        => await _dbContext.Wallets.FindAsync(id);

    public async Task<Wallet?> GetByPlayerId(Guid playerId)
        => await _dbContext.Wallets.FirstOrDefaultAsync(w => w.PlayerId == playerId);

    public async Task<Wallet> CreateWalletAsync(Guid playerId)
    {
        var wallet = await GetByPlayerId(playerId);

        if (wallet != null)
        {
            return wallet;
        }

        wallet = new Wallet(
            Guid.NewGuid(),
            playerId,
            WalletConstants.InitialBalance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        return wallet;
    }

    public async Task DepositAsync(Wallet wallet, decimal amount)
    {
        if (wallet is null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        wallet.UpdateAmount(amount);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<decimal> WithdrawAsync(Wallet wallet, decimal amount)
    {
        if (wallet is null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        if (wallet.Balance - amount < 0)
        {
            throw new InsufficientBalanceException(wallet.Id, amount);
        }

        wallet.UpdateAmount(-amount);
        await _dbContext.SaveChangesAsync();
        
        return amount;
    }
}