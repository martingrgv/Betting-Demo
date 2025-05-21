using BettingGame.Domain.Entities;
using BettingGame.Domain.ValueObjects;
using BettingGame.Shared.Exceptions;

namespace BettingGame.Tests.DomainTests;

[TestFixture]
public class WalletTests
{
    private const int InitialWalletBalance = 0;
    private Wallet _wallet;

    [SetUp]
    public void SetUp()
    {
        var money = Money.Create(InitialWalletBalance);
        _wallet = Wallet.Create(Guid.NewGuid(), money);
    }

    [Test]
    public void Constructor_ShouldSet_InitialBalance()
    {
        var initialMoney = Money.Create(InitialWalletBalance);
        
        Assert.NotNull(_wallet);
        Assert.AreEqual(initialMoney, _wallet.Balance);
    }

    [Test]
    public void Deposit_PositiveMoney_ShouldUpdateBalance()
    {
        // Arrange
        var depositMoney = Money.Create(100);
        
        // Act
        _wallet.Deposit(depositMoney);
        
        // Assert
        Assert.AreEqual(depositMoney, _wallet.Balance);
    }

    [Test]
    public void Deposit_ZeroMoney_ShouldUpdateBalance()
    {
        // Arrange
        var depositMoney = Money.Create(100);
        var depositMoney2 = Money.Create(0);
        
        // Act
        _wallet.Deposit(depositMoney);
        _wallet.Deposit(depositMoney2);
        var resultMoney = depositMoney + depositMoney2;
        
        // Assert
        Assert.AreEqual(resultMoney, _wallet.Balance);
    }
    
    [Test]
    public void Deposit_NegativeMoney_ShouldThrowException()
    {
        // Arrange
        var depositMoney = Money.Create(-100);
        
        // Act
        void ErrorAction() => _wallet.Deposit(depositMoney);

        // Assert
        Assert.Throws<DomainException>(ErrorAction);
    }

    [Test]
    public void Withdraw_PositiveMoney_ShouldLowerBalance()
    {
        // Arrange
        var depositMoney = Money.Create(200);
        var withdrawMoney = Money.Create(100);
        
        // Act
        _wallet.Deposit(depositMoney);
        var expectedMoney = _wallet.Balance - withdrawMoney;
        _wallet.Withdraw(withdrawMoney);
        
        // Assert
        Assert.AreEqual(expectedMoney, _wallet.Balance);
    }

    [Test]
    public void Withdraw_Zero_ShouldNotLowerBalance()
    {
        // Arrange
        var depositMoney = Money.Create(200);
        var withdrawMoney = Money.Create(0);
        
        // Act
        _wallet.Deposit(depositMoney);
        var expectedMoney = _wallet.Balance - withdrawMoney;
        _wallet.Withdraw(withdrawMoney);
        
        // Assert
        Assert.AreEqual(expectedMoney, _wallet.Balance);
    }

    [Test]
    public void Withdraw_NegativeMoney_ShouldThrowException()
    {
        // Arrange
        var withdrawMoney = Money.Create(-100);
        
        // Act
        void ErrorAction() => _wallet.Withdraw(withdrawMoney);
        
        // Assert
        Assert.Throws<DomainException>(ErrorAction);
    }
}