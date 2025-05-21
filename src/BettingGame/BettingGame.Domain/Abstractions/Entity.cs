namespace BettingGame.Domain.Abstractions;

public abstract class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; }
}