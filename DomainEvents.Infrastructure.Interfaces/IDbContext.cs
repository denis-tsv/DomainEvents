using DomainEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbContext
{
    bool IsTransactionStarted { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    DbSet<Account> Accounts { get; }

    DbSet<AccountGroup> AccountGroups { get; }

    DbSet<AccountGroupAccount> AccountGroupAccounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}