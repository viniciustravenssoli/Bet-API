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
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BetContext).Assembly);

            modelBuilder.Entity<Domain.Entities.Bet>()
                .HasOne(b => b.Home)
                .WithMany(t => t.Bets)
                .HasForeignKey(b => b.HomeId);

            modelBuilder.Entity<Domain.Entities.Bet>()
                .HasOne(b => b.Visitor)
                .WithMany()
                .HasForeignKey(b => b.VisitorId);
            modelBuilder.Entity<Domain.Entities.Bet>()
                .Property(b => b.WinnerId)
                .IsRequired(false); // Permitindo valores nulos

    }
}
