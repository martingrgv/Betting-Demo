namespace BettingGame.Services;

public class WalletService : IWalletService
{
    private readonly IList<Wallet> _wallets = [];
    private const int InitialBalance = 0;

    public Task<Wallet?> GetByPlayerIdAsync(Guid playerId)
        => Task.FromResult(_wallets.FirstOrDefault(w => w.PlayerId == playerId));

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

        _wallets.Add(wallet);

        return wallet;
    }

    public async Task DepositAsync(Wallet wallet, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        
        await UpdateBalanceAsync(wallet, amount);
    }

    public async Task<decimal> WithdrawAsync(Wallet wallet, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        InsufficientBalanceException.ThrowIfBalanceNegative(wallet, amount);
        
        var withdrawAmount = await UpdateBalanceAsync(wallet, -amount);

        return withdrawAmount;
    }

    private Task<decimal> UpdateBalanceAsync(Wallet wallet, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(wallet);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(amount, decimal.MaxValue);

        if (!_wallets.Any(w => w == wallet))
        {
            throw new InvalidOperationException($"Wallet doesn't exist and cannot be updated! WalletId: {wallet.Id}; PlayerId: {wallet.PlayerId}");
        }

        wallet.UpdateAmount(amount);
        return Task.FromResult(amount);
    }
}