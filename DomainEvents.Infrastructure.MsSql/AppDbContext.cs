using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.MsSql;

public class AppDbContext : DbContext, IDbContext
{
    private readonly IPublisher _publisher;

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
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

    // can be implemented as interceptor
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var notifications = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(x => x.Entity.FetchNotifications())
            .ToList();
        
        foreach (var notification in notifications)
        {
            await _publisher.Publish(notification, cancellationToken);
        }

        return result;
    }
}