using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var accountGroup = await _dbContext.AccountGroups
            .Include(x => x.Accounts) //can't include filter because need to check accounts count
            .FirstOrDefaultAsync(x => x.Id == request.AccountGroupId, cancellationToken);

        accountGroup!.RemoveAccount(request.AccountId);

        if (!accountGroup.Accounts.Any())
        {
            _dbContext.AccountGroups.Remove(accountGroup);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}