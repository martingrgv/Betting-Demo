using BettingGame.Abstractions;
using BettingGame.Commands;
using BettingGame.Common;
using BettingGame.Constants;
using BettingGame.Data.Models;
using BettingGame.Exceptions;
using Moq;

namespace BettingGame.Tests.Commands;

[TestFixture]
public class BetCommandTests
{
    private Mock<IWalletService> _walletServiceMock;
    private Mock<IBetService> _betServiceMock;

    [SetUp]
    public void SetUp()
    {
        _walletServiceMock = new Mock<IWalletService>();
        _betServiceMock = new Mock<IBetService>();
    }
    
    [Test]
    public async Task Execute_GameWon_IncreasesBalanceAndReturnsSuccess()
    {
        var betAmount = 5;
        var winAmount = 10;
        string[] args = [ "bet", betAmount.ToString() ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 50);
        var command = CreateCommand(wallet);
        var gameResult = new GameResult(IsWin: true, BetAmount: betAmount, WinAmount: winAmount);

        _betServiceMock.Setup(x => x.Bet(betAmount))
            .Returns(gameResult);
        
        var result = await command.Execute(args);
        
        _betServiceMock.Verify(x => x.Bet(betAmount), Times.Once);
        _walletServiceMock.Verify(x => x.WithdrawAsync(wallet, betAmount), Times.Never);
        _walletServiceMock.Verify(x => x.DepositAsync(wallet, winAmount), Times.Once);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Exception, Is.Null);
    }
    
    [Test]
    public async Task Execute_GameLost_LowersBalanceAndReturnsSuccess()
    {
        var betAmount = 5;
        string[] args = [ "bet", betAmount.ToString() ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 50);
        var command = CreateCommand(wallet);
        var gameResult = new GameResult(IsWin: false, BetAmount: betAmount, WinAmount: 0);

        _betServiceMock.Setup(x => x.Bet(betAmount))
            .Returns(gameResult);
        
        var result = await command.Execute(args);
        
        _betServiceMock.Verify(x => x.Bet(betAmount), Times.Once);
        _walletServiceMock.Verify(x => x.WithdrawAsync(wallet, betAmount), Times.Once);
        _walletServiceMock.Verify(x => x.DepositAsync(wallet, betAmount), Times.Never);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Exception, Is.Null);
    }
    
    [Test]
    public async Task Execute_InsufficientBalance_ReturnsResultFailure()
    {
        string[] args = [ "bet", "100" ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 50);
        var command = CreateCommand(wallet);

        var result = await command.Execute(args);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf(typeof(InsufficientBalanceException)));
    }
    
    [Test]
    public async Task Execute_NegativeBetAmount_ReturnsResultFailure()
    {
        string[] args = [ "bet", "-0.01" ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 0);
        var command = CreateCommand(wallet);

        var result = await command.Execute(args);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf(typeof(ArgumentOutOfRangeException)));
    }

    [Test]
    public async Task Execute_InvalidAmountArgument_ReturnsResultFailure()
    {
        string[] args = [ "bet", "test" ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 0);
        var command = CreateCommand(wallet);

        var result = await command.Execute(args);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.EqualTo(MessageConstants.CommonErrorMessage));
        Assert.That(result.Exception, Is.TypeOf(typeof(FormatException)));
    }

    [Test]
    public async Task Execute_NoArgs_ReturnsResultFailure()
    {
        string[] args = [];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 0);
        var command = CreateCommand(wallet);

        var result = await command.Execute(args);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Is.EqualTo(MessageConstants.CommonErrorMessage));
        Assert.That(result.Exception, Is.TypeOf(typeof(IndexOutOfRangeException)));
    }

    private ICommand CreateCommand(Wallet wallet)
        => new BetCommand(_betServiceMock.Object, _walletServiceMock.Object, wallet);
}