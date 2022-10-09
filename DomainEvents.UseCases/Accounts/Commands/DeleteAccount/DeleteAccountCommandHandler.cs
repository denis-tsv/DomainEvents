using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : AsyncRequestHandler<DeleteAccountCommand>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteAccountCommandHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    protected override async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Accounts
            .Where(x => x.Id == request.AccountId)
            .BatchUpdateAsync(x => new Account { IsDeleted = true }, cancellationToken: cancellationToken);

        var accountGroupIds = await _dbContext.AccountGroupAccounts
            .Where(x => x.AccountId == request.AccountId)
            .Select(x => x.AccountGroupId)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var accountGroupId in accountGroupIds)
        {
            await _sender.Send(
                new RemoveAccountFromGroupCommand { AccountGroupId = accountGroupId, AccountId = request.AccountId },
                cancellationToken);
        }
    }
}