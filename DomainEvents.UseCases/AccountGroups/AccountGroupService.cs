using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;

namespace DomainEvents.UseCases.AccountGroups;

public class AccountGroupService
{
    public void RemoveAccountFromAccountGroup(AccountGroup accountGroup, int accountId, IDbContext dbContext)
    {
        accountGroup.Accounts.RemoveAll(x => x.AccountId == accountId);

        if (!accountGroup.Accounts.Any())
        {
            dbContext.AccountGroups.Remove(accountGroup);
        }
    }
}