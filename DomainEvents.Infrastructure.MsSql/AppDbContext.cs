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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var notifications = new List<INotification>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Modified or EntityState.Added or EntityState.Deleted)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    notifications.AddRange(entity.FetchNotifications());
                }
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            await _publisher.Publish(notification, cancellationToken);
        }

        return result;
    }
}