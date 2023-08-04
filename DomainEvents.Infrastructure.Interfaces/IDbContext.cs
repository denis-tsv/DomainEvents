using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbContext : IDbSets
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}