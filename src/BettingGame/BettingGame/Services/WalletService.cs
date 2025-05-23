using BettingGame.Data;
using Microsoft.EntityFrameworkCore;

namespace BettingGame.Services;

public class WalletService(BettingDbContext dbContext) : IWalletService
{
   private const int InitialBalance = 0;
    
    public async Task<Wallet?> GetById(Guid id)
        => await dbContext.Wallets.FindAsync(id);

    public async Task<Wallet?> GetByPlayerId(Guid playerId)
        => await dbContext.Wallets.FirstOrDefaultAsync(w => w.PlayerId == playerId);

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
            InitialBalance);

        dbContext.Wallets.Add(wallet);
        await dbContext.SaveChangesAsync();

        return wallet;
    }

    public async Task DepositAsync(Wallet wallet, decimal amount)
    {
        if (wallet is null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }
        
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amount)} must be a positive number.");
        }

        wallet.UpdateAmount(amount);
        await dbContext.SaveChangesAsync();
    }

    public async Task<decimal> WithdrawAsync(Wallet wallet, decimal amount)
    {
        if (wallet is null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amount)} must be a positive number.");
        }

        if (wallet.Balance - amount < 0)
        {
            throw new InsufficientBalanceException(wallet.Id, amount);
        }

        wallet.UpdateAmount(-amount);
        await dbContext.SaveChangesAsync();
        
        return amount;
    }
}