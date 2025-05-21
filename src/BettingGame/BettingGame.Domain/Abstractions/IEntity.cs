namespace BettingGame.Domain.Abstractions;

public interface IEntity<TId>
{
    TId Id { get; set; }
}