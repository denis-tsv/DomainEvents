using DomainEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbContext
{
    Task<IDbContextTransaction> GetTransactionAsync(CancellationToken cancellationToken);

    DbSet<Account> Accounts { get; }

    DbSet<AccountGroup> AccountGroups { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}