using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountGroupGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups.EventHandlers;

public class AccountDeletedEventHandler : INotificationHandler<AccountDeletedEvent>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public AccountDeletedEventHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task Handle(AccountDeletedEvent notification, CancellationToken cancellationToken)
    {
        var accountGroupIds = await _dbContext.AccountGroups
            .Where(x => x.Accounts.Any(a => a.AccountId == notification.AccountId))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        foreach (var accountGroupId in accountGroupIds)
        {
            await _sender.Send(
                new RemoveAccountFromGroupCommand
                    { AccountId = notification.AccountId, AccountGroupId = accountGroupId }, cancellationToken);
        }
    }
}