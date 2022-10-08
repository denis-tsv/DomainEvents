﻿using DomainEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.Infrastructure.Interfaces;

public interface IDbContext
{
    DbSet<Account> Accounts { get; }

    DbSet<AccountGroup> AccountGroups { get; }

    DbSet<AccountGroupAccount> AccountGroupAccounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}