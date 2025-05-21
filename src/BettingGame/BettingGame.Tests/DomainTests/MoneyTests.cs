using BettingGame.Domain.ValueObjects;

namespace BettingGame.Tests.DomainTests;

[TestFixture]
public class MoneyTests
{
    [Test]
    public void CreateMoney_ShouldReturnMoneyObj()
    {
        // Arrange
        int amount = 100;
        
        // Act
        var money = Money.Create(amount);

        // Assert
        Assert.IsNotNull(money);
        Assert.AreEqual(amount, money.Amount);
    }
    
    [Test]
    public void CompareSameAmounts_DifferentObjs_ShouldBeEqual()
    {
        // Arrange
        int amount = 100;
        
        // Act
        var money = Money.Create(amount);
        var money2 = Money.Create(amount);

        // Assert
        Assert.AreEqual(money, money2);
        Assert.AreNotSame(money, money2);
    }
    
    [Test]
    public void CompareDifferentAmounts_DifferentObjs_ShouldNotBeEqual()
    {
        // Arrange
        int amount = 100;
        int amount2 = 200;
        
        // Act
        var money = Money.Create(amount);
        var money2 = Money.Create(amount2);

        // Assert
        Assert.AreNotEqual(money, money2);
        Assert.AreNotSame(money, money2);
    }
    
    [Test]
    public void Add_MoneyObjs_ShouldIncreaseAmount()
    {
        // Arrange
        int amount = 100;
        int amount2 = 200;
        
        // Act
        var money = Money.Create(amount);
        var money2 = Money.Create(amount2);

        var expectedResult = amount + amount2;
        var resultMoney = money + money2;

        // Assert
        Assert.AreEqual(expectedResult, resultMoney.Amount);
    }
    
    [Test]
    public void Subtract_MoneyObjs_ShouldLowerAmount()
    {
        // Arrange
        int amount = 100;
        int amount2 = 200;
        
        // Act
        var money = Money.Create(amount);
        var money2 = Money.Create(amount2);

        var expectedResult = amount - amount2;
        var resultMoney = money - money2;

        // Assert
        Assert.AreEqual(expectedResult, resultMoney.Amount);
    }
}