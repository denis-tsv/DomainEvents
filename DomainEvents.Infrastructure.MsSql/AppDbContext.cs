using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.MsSql;

public class AppDbContext : DbContext, IDbContext
{
    private readonly IPublisher _publisher;
    private bool _isTransactionStarted;

    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>()
            .HasKey(x => new { x.ProductId, x.CategoryId });

        modelBuilder.Entity<Category>()
            .Property(x => x.Name)
            .HasMaxLength(64);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_isTransactionStarted)
            return Task.FromResult<IDbContextTransaction>(new FakeDbContextTransaction());

        _isTransactionStarted = true;

        return Database.BeginTransactionAsync(cancellationToken);
    }

    // can be implemented as interceptor
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        while(true)
        {
            var notifications = ChangeTracker.Entries<BaseEntity>()
                .SelectMany(x => x.Entity.FetchNotifications())
                .ToList();
            if (!notifications.Any()) break;

            foreach (var notification in notifications)
            {
                await _publisher.Publish(notification, cancellationToken);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private class FakeDbContextTransaction : IDbContextTransaction
    {
        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        public void Commit()
        {
        }

        public Task CommitAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Rollback()
        {
        }

        public Task RollbackAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Guid TransactionId { get; }
    }
}