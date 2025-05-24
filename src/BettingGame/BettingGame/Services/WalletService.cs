using BettingGame.Data;
using Microsoft.EntityFrameworkCore;

namespace BettingGame.Services;

public class WalletService(BettingDbContext dbContext) : IWalletService
{
   private const int InitialBalance = 0;
    
    public async Task<Wallet?> GetByIdAsync(Guid id)
        => await dbContext.Wallets.FindAsync(id);

    public async Task<Wallet?> GetByPlayerIdAsync(Guid playerId)
        => await dbContext.Wallets.FirstOrDefaultAsync(w => w.PlayerId == playerId);

    public async Task<Wallet> CreateWalletAsync(Guid playerId)
    {
        var wallet = await GetByPlayerIdAsync(playerId);
        if (wallet != null)
        {
            return wallet;
        }

        wallet = new Wallet(
            Guid.NewGuid(),
            playerId,
            InitialBalance);

        dbContext.Wallets.Add(wallet);
        await dbContext.SaveChangesAsync();

        return wallet;
    }

    public async Task DepositAsync(Wallet wallet, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(wallet);
        ArgumentOutOfRangeException.ThrowIfNegative(amount);

        dbContext.Attach(wallet);
        wallet.UpdateAmount(amount);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<decimal> WithdrawAsync(Wallet wallet, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(wallet);
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        InsufficientBalanceException.ThrowIfBalanceNegative(wallet, amount);

        dbContext.Attach(wallet);
        wallet.UpdateAmount(-amount);
        
        await dbContext.SaveChangesAsync();
        return amount;
    }
}