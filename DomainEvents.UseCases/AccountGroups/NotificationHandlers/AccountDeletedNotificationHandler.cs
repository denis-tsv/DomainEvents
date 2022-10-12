using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups.NotificationHandlers;

public class AccountDeletedNotificationHandler : INotificationHandler<AccountDeletedNotification>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public AccountDeletedNotificationHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task Handle(AccountDeletedNotification notification, CancellationToken cancellationToken)
    {
        var accountGroupIds = await _dbContext.AccountGroupAccounts
            .Where(x => x.AccountId == notification.AccountId)
            .Select(x => x.AccountGroupId)
            .Distinct()
            .ToListAsync(cancellationToken);

        // Command handlers shares not thread safe DbContext, so we send command sequently, but not in parallel
        foreach (var accountGroupId in accountGroupIds)
        {
            await _sender.Send(
                new RemoveAccountFromGroupCommand { AccountGroupId = accountGroupId, AccountId = notification.AccountId },
                cancellationToken);
        }
    }
}