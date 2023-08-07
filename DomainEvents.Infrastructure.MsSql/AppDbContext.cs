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

    public bool IsTransactionStarted { get; private set; }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        IsTransactionStarted = true;

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
}