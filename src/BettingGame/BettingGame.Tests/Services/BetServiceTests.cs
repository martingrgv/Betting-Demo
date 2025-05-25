namespace BettingGame.Tests.Services;

[TestFixture]
public class BetServiceTests
{
    private const decimal MinBetAmount = 1;
    private const decimal MaxBetAmount = 10;
    private Mock<IBetOutcomeResolver> _betResolverMock;
    private Mock<IBetOutcomeStrategy> _betStrategyMock;
    private IBetService _betService;

    [SetUp]
    public void SetUp()
    {
        _betResolverMock = new Mock<IBetOutcomeResolver>();
        _betStrategyMock = new Mock<IBetOutcomeStrategy>();
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

        _betStrategyMock.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategyMock.Object);

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

        _betStrategyMock.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategyMock.Object);

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
        
        _betStrategyMock.Setup(x => x.CalculateOutcome(betAmount))
            .Returns(winAmount);
        _betResolverMock.Setup(x => x.ResolveBetStrategy())
            .Returns(_betStrategyMock.Object);

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