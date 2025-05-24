using BettingGame.Abstractions;
using BettingGame.Common;
using BettingGame.Services;
using Moq;

namespace BettingGame.Tests.Services;

[TestFixture]
public class BetServiceTests
{
    private const decimal MinBetAmount = 1;
    private const decimal MaxBetAmount = 10;
    private Mock<IBetOutcomeResolver> _betResolverMock;
    private Mock<IBetOutcomeStrategy> _betStrategy;
    private IBetService _betService;

    [SetUp]
    public void SetUp()
    {
        _betResolverMock = new Mock<IBetOutcomeResolver>();
        _betStrategy = new Mock<IBetOutcomeStrategy>();
        _betService = new BetService(_betResolverMock.Object);
    }

    [Test]
    public void Bet_MultiplierWinStrategy_ReturnsMultiplierWinGameResult()
    {
        var betAmount = 5;
        var multiplier = 5.5m;
        var winAmount = betAmount * multiplier;
        var gameResult = new GameResult(
            IsWin: true,
            BetAmount: betAmount,
            WinAmount: winAmount);

        _betStrategy.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategy.Object);

        var result = _betService.Bet(betAmount);
        
        Assert.That(gameResult, Is.EqualTo(result));
    }

    [Test]
    public void Bet_DoubleWinStrategy_ReturnsDoubleWinGameResult()
    {
        var betAmount = 5;
        var winAmount = 10;
        var gameResult = new GameResult(
            IsWin: true,
            BetAmount: betAmount,
            WinAmount: winAmount);

        _betStrategy.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategy.Object);

        var result = _betService.Bet(betAmount);
        
        Assert.That(gameResult, Is.EqualTo(result));
    }

    [Test]
    public void Bet_LoseStrategy_ReturnsLoseGameResult()
    {
        var betAmount = 5;
        var winAmount = 0;
        var gameResult = new GameResult(
            IsWin: false,
            BetAmount: betAmount,
            WinAmount: winAmount);
        
        _betStrategy.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategy.Object);

        var result = _betService.Bet(betAmount);
        
        Assert.That(gameResult, Is.EqualTo(result));
    }

    [Test]
    public void Bet_HigherThanMaxRange_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _betService.Bet(MaxBetAmount + 0.01m));
    }
    
    [Test]
    public void Bet_LowerThanMinRange_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _betService.Bet(MinBetAmount - 0.01m));
    }

    [Test]
    public void Bet_NegativeAmount_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _betService.Bet(-1));
    }
}