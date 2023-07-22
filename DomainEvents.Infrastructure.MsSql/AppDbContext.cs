using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.MsSql;

public class AppDbContext : DbContext, IDbContext
{
    private bool _isTransactionStarted;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>()
            .HasKey(x => new { x.ProductId, x.CategoryId});

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
}

internal class FakeDbContextTransaction : IDbContextTransaction
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