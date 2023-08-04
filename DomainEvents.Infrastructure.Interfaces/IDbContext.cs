using DomainEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbContext
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    DbSet<Product> Products { get; }

    DbSet<Category> Categories { get; }

    DbSet<ProductCategory> ProductCategories { get; }
    DbSet<Message> Messages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}