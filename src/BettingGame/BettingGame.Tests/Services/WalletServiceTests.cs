using System.Data;
using BettingGame.Abstractions;
using BettingGame.Data;
using BettingGame.Data.Models;
using BettingGame.Exceptions;
using BettingGame.Services;
using Microsoft.EntityFrameworkCore;

namespace BettingGame.Tests.Services;

[TestFixture]
public class WalletServiceTests
{
    private BettingDbContext _dbContext;
    private IWalletService _walletService;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<BettingDbContext>()
            .UseInMemoryDatabase("BettingDb")
            .Options;

        _dbContext = new BettingDbContext(options);
        _walletService = new WalletService(_dbContext);
    }

    [Test]
    public async Task GetById_WalletExists_ReturnsWallet()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var wallet = new Wallet(walletId, playerId, balance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        var result = await _walletService.GetByIdAsync(walletId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(walletId));
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(balance));
        });
    }

    [Test]
    public async Task GetById_WalletNotExisting_ReturnsNull()
    {
        var walletId = Guid.NewGuid();

        var result = await _walletService.GetByIdAsync(walletId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByPlayerId_PlayerExists_ReturnsWallet()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var wallet = new Wallet(walletId, playerId, balance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        var result = await _walletService.GetByPlayerIdAsync(playerId);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(walletId));
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(balance));
        });
    }


    [Test]
    public async Task GetByPlayerId_PlayerNotExisting_ReturnsNull()
    {
        var playerId = Guid.NewGuid();

        var result = await _walletService.GetByPlayerIdAsync(playerId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateWalletAsync_WalletExists_ReturnsExistingWallet()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var wallet = new Wallet(walletId, playerId, balance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        var result = await _walletService.CreateWalletAsync(playerId);
        int walletsCount = _dbContext.Wallets.Count();

        Assert.That(result, Is.Not.Null);
        Assert.That(walletsCount, Is.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(walletId));
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(balance));
        });
    }

    [Test]
    public async Task CreateWalletAsync_WalletNotExisting_CreatesAndReturnsWallet()
    {
        var playerId = Guid.NewGuid();

        var result = await _walletService.CreateWalletAsync(playerId);
        var existsInDb = await _dbContext.Wallets.AnyAsync(w => w.PlayerId == playerId);

        Assert.That(result, Is.Not.Null);
        Assert.That(existsInDb, Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(result.PlayerId, Is.EqualTo(playerId));
            Assert.That(result.Balance, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task DepositAsync_DepositsAmountToWallet()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var depositAmount = 200;
        var wallet = new Wallet(walletId, playerId, balance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        Assert.DoesNotThrowAsync(() => _walletService.DepositAsync(wallet, depositAmount));
        Assert.That(async () => (await _dbContext.Wallets.FindAsync(walletId))!.Balance,
            Is.EqualTo(balance + depositAmount));
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

        Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _walletService.DepositAsync(wallet, depositAmount));
    }
    
    [Test]
    public void DepositAsync_NullWallet_ThrowsException()
    {
         Assert.ThrowsAsync<ArgumentNullException>(() => _walletService.DepositAsync(null, 0));
    }
    
    [Test]
    public async Task WithdrawAsync_HasBalanceToWithdraw_LowersBalance()
    {
        var walletId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var balance = 100m;
        var withdrawAmount = 50;
        var wallet = new Wallet(walletId, playerId, balance);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync();

        Assert.DoesNotThrowAsync(() => _walletService.WithdrawAsync(wallet, withdrawAmount));
        Assert.That(async () => (await _dbContext.Wallets.FindAsync(walletId))!.Balance,
            Is.EqualTo(balance - withdrawAmount));
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

        Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _walletService.WithdrawAsync(wallet, depositAmount));
    }
        
    [Test]
    public void WithdrawAsync_NullWallet_ThrowsException()
    {
         Assert.ThrowsAsync<ArgumentNullException>(() => _walletService.WithdrawAsync(null, 0));
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
}