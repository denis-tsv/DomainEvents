using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using EFCore.BulkExtensions;
using MediatR;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : AsyncRequestHandler<DeleteAccountCommand>
{
    private readonly IDbContext _dbContext;

    public DeleteAccountCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Accounts
            .Where(x => x.Id == request.AccountId)
            .BatchUpdateAsync(x => new Account { IsDeleted = true }, cancellationToken: cancellationToken);

        await _dbContext.AccountGroupAccounts
            .Where(x => x.AccountId == request.AccountId)
            .BatchDeleteAsync(cancellationToken);

        await _dbContext.AccountGroups
            .Where(x => !x.Accounts.Any())
            .BatchDeleteAsync(cancellationToken);
    }
}