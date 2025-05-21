using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BettingGame.Data.Persistence;

public class BettingDbContext(DbContextOptions<BettingDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets => Set<Wallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}