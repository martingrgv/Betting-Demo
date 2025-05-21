using BettingGame.Domain.Abstractions;

namespace BettingGame.Domain.Entities;

public class Player : Entity<Guid>
{
    public Player(Guid id, Guid walletId)
    {
        Id = id;
        WalletId = walletId;
    }
    
    public Guid WalletId { get; }

    public static Player Create(Guid id, Guid walletId)
        => new(id, walletId);
}