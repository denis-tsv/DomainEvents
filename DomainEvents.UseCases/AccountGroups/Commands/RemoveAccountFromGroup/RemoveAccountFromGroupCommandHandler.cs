using DomainEvents.Infrastructure.Interfaces;
using EFCore.BulkExtensions;
using MediatR;

namespace DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;

public class RemoveAccountFromGroupCommandHandler : AsyncRequestHandler<RemoveAccountFromGroupCommand>
{
    private readonly IDbContext _dbContext;

    public RemoveAccountFromGroupCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(RemoveAccountFromGroupCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.AccountGroupAccounts
            .Where(x => x.AccountGroupId == request.AccountGroupId && x.AccountId == request.AccountId)
            .BatchDeleteAsync(cancellationToken);

        await _dbContext.AccountGroups
            .Where(x => x.Id == request.AccountGroupId && !x.Accounts.Any())
            .BatchDeleteAsync(cancellationToken);
    }
}