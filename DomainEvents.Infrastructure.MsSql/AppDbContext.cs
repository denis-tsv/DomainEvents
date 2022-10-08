using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.Infrastructure.MsSql;

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Account> Accounts { get; set; } = null!;

    public DbSet<AccountGroup> AccountGroups { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountGroupAccount>()
            .HasKey(x => new { x.AccountId, x.AccountGroupId });
    }
}