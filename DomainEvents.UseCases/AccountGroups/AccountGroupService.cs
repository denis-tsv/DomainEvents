using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;

namespace DomainEvents.UseCases.AccountGroups;

public class AccountGroupService
{
    private readonly IDbContext _dbContext;

    public AccountGroupService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void RemoveAccountFromGroup(AccountGroup accountGroup, int accountId)
    {
        accountGroup.RemoveAccount(accountId);

        if (!accountGroup.Accounts.Any())
        {
            _dbContext.AccountGroups.Remove(accountGroup);
        }
    }
}