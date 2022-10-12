using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.MsSql;

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;

    public DbSet<AccountGroup> AccountGroups { get; set; } = null!;

    public DbSet<AccountGroupAccount> AccountGroupAccounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountGroupAccount>()
            .HasKey(x => new { x.AccountId, x.AccountGroupId });

        modelBuilder.Entity<AccountGroup>()
            .Property(x => x.Name)
            .HasMaxLength(64);
    }

    public bool IsTransactionStarted { get; private set; }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        IsTransactionStarted = true;

        return Database.BeginTransactionAsync(cancellationToken);
    }
}