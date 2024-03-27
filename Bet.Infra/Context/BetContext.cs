using Bet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bet.Infra.Context;
public class BetContext : DbContext
{
    public BetContext(DbContextOptions<BetContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Domain.Entities.Bet> Bets { get; set; }
    public DbSet<UserBet> UserBets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BetContext).Assembly);

    }
}
