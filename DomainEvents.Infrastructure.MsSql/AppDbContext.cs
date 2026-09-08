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

    public bool IsTransactionStarted { get; private set; }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        IsTransactionStarted = true;

        return Database.BeginTransactionAsync(cancellationToken);
    }
}