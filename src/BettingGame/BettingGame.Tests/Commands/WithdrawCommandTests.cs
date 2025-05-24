using BettingGame.Abstractions;
using BettingGame.Commands;
using BettingGame.Constants;
using BettingGame.Data.Models;
using Moq;

namespace BettingGame.Tests.Commands;

[TestFixture]
public class WithdrawCommandTests
{
    private Mock<IWalletService> _walletServiceMock;

    [SetUp]
    public void SetUp()
    {
        _walletServiceMock = new Mock<IWalletService>();
    }

    [Test]
    public async Task Execute_PositiveAmount_WithdrawsAndReturnsSuccess()
    {
        var withdrawAmount = 50;
        string[] args = [ "withdraw", withdrawAmount.ToString() ];
        var wallet = new Wallet(Guid.NewGuid(), Guid.NewGuid(), 0);
        var command = CreateCommand(wallet);

        var result = await command.Execute(args);
        
        _walletServiceMock.Verify(x => x.DepositAsync(wallet, withdrawAmount), Times.Never);
        _walletServiceMock.Verify(x => x.WithdrawAsync(wallet, withdrawAmount), Times.Once);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Exception, Is.Null);
    }
    
    [Test]
    public async Task Execute_NegativeAmount_ReturnsResultFailure()
    {
        string[] args = [ "withdraw", "-0.01" ];
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
        string[] args = [ "withdraw", "test" ];
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
        => new WithdrawCommand(_walletServiceMock.Object, wallet);
}