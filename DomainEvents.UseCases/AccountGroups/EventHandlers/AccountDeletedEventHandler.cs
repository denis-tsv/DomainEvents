using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups.EventHandlers;

public class AccountDeletedEventHandler : INotificationHandler<AccountDeletedEvent>
{
    private readonly IDbContext _dbContext;
    private readonly AccountGroupService _accountGroupService;

    public AccountDeletedEventHandler(IDbContext dbContext, AccountGroupService accountGroupService)
    {
        _dbContext = dbContext;
        _accountGroupService = accountGroupService;
    }

    public async Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
    {
        var accountGroups = await _dbContext.AccountGroups
            .Where(x => x.Accounts.Any(a => a.AccountId == notification.AccountId))
            .Include(x => x.Accounts)
            .ToListAsync(cancellationToken);

        foreach (var accountGroup in accountGroups)
        {
            _accountGroupService.RemoveAccountFromAccountGroup(accountGroup, notification.AccountId, _dbContext);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}