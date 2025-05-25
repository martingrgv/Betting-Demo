namespace BettingGame.Tests.Services;

[TestFixture]
public class WalletServiceTests
{
    private const int InitialBalance = 0;
    private IWalletService _walletService;

    [SetUp]
    public void SetUp()
    {
        _walletService = new WalletService();
    }

    [Test]
    public async Task GetByPlayerId_PlayerHasWallet_ReturnsExistingWallet()
    {
        var playerId = Guid.NewGuid();
        var wallet = await _walletService.CreateWalletAsync(playerId);
        
        var result = await _walletService.GetByPlayerIdAsync(playerId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(wallet.Id));
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(wallet.Balance));
        });
    }

    [Test]
    public async Task GetByPlayerId_PlayerDoesNotHaveWallet_ReturnsNull()
    {
        var playerId = Guid.NewGuid();

        var result = await _walletService.GetByPlayerIdAsync(playerId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateWalletAsync_WalletExists_ReturnsExistingWallet()
    {
        var playerId = Guid.NewGuid();
        var wallet = await _walletService.CreateWalletAsync(playerId);

        var result = await _walletService.CreateWalletAsync(playerId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(wallet.Id));
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(wallet.Balance));
        });
    }

    [Test]
    public async Task CreateWalletAsync_WalletNotExisting_CreatesAndReturnsWallet()
    {
        var playerId = Guid.NewGuid();

        var result = await _walletService.CreateWalletAsync(playerId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(InitialBalance));
        });
    }

    [Test]
    public async Task DepositAsync_DepositsAmountToWallet()
    {
        var playerId = Guid.NewGuid();
        var initialBalance = 0;
        var depositAmount = 200m;
        var wallet = await _walletService.CreateWalletAsync(playerId);

        Assert.DoesNotThrowAsync(() => _walletService.DepositAsync(wallet, depositAmount));
        Assert.That(wallet.Balance, Is.EqualTo(initialBalance + depositAmount));
    }

    [Test]
    public void DepositAsync_GreaterThanMaxAmount_ThrowsException()
    {
        var amount = 10;
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), decimal.MaxValue);

        Assert.ThrowsAsync<InvalidOperationException>(() => _walletService.DepositAsync(wallet, amount));
    }
    
    [Test]
    public void DepositAsync_NegativeAmount_ThrowsException()
    {
        var negativeAmount = -200;
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 100m);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _walletService.DepositAsync(wallet, negativeAmount));
    }

    [Test]
    public void DepositAsync_NotExistingWallet_ThrowsException()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var depositAmount = 200;
        var wallet = new Wallet(walletId, playerId, balance);

        Assert.ThrowsAsync<InvalidOperationException>(() => _walletService.DepositAsync(wallet, depositAmount));
    }
    
    [Test]
    public void DepositAsync_NullWallet_ThrowsException()
    {
         Assert.ThrowsAsync<ArgumentNullException>(() => _walletService.DepositAsync(null, 0));
    }
    
    [Test]
    public async Task WithdrawAsync_HasBalanceToWithdraw_LowersBalance()
    {
        var playerId = Guid.NewGuid();
        var initialBalance = 100m;
        var withdrawAmount = 50m;
        var wallet = await _walletService.CreateWalletAsync(playerId);
        wallet.UpdateAmount(initialBalance);

        Assert.DoesNotThrowAsync(() => _walletService.WithdrawAsync(wallet, withdrawAmount));
        Assert.That(wallet.Balance, Is.EqualTo(initialBalance - withdrawAmount));
    }
    
    [Test]
    public async Task WithdrawAsync_GreaterThanMaxAmountBalance_LowersBalance()
    {
        var playerId = Guid.NewGuid();
        var initialBalance = decimal.MaxValue;
        var withdrawAmount = 1;
        var wallet = await _walletService.CreateWalletAsync(playerId);
        wallet.UpdateAmount(initialBalance);

        Assert.DoesNotThrowAsync(() => _walletService.WithdrawAsync(wallet, withdrawAmount));
        Assert.That(wallet.Balance, Is.EqualTo(initialBalance - withdrawAmount));
    }
    
    [Test]
    public void WithdrawAsync_InsufficientBalance_ThrowsException()
    {
        var balance = 100m;
        var withdrawAmount = 200;
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), balance);

        Assert.ThrowsAsync<InsufficientBalanceException>(() => _walletService.WithdrawAsync(wallet, withdrawAmount));
    }
    
    [Test]
    public void WithdrawAsync_NegativeAmount_ThrowsException()
    {
        var negativeAmount = -200;
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 100m);

        Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _walletService.WithdrawAsync(wallet, negativeAmount));
    }
    
    [Test]
    public void WithdrawAsync_NotExistingWallet_ThrowsException()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var depositAmount = 50;
        var wallet = new Wallet(walletId, playerId, balance);

        Assert.ThrowsAsync<InvalidOperationException>(() => _walletService.WithdrawAsync(wallet, depositAmount));
    }
        
    [Test]
    public void WithdrawAsync_NullWallet_ThrowsException()
    {
         Assert.ThrowsAsync<NullReferenceException>(() => _walletService.WithdrawAsync(null, 0));
    }
}