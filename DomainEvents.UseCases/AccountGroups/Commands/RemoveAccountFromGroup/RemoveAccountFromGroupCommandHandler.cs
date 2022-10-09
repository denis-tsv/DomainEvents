using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;

public class RemoveAccountFromGroupCommandHandler : AsyncRequestHandler<RemoveAccountFromGroupCommand>
{
    private readonly IDbContext _dbContext;
    private readonly AccountGroupService _accountGroupService;

    public RemoveAccountFromGroupCommandHandler(IDbContext dbContext, AccountGroupService accountGroupService)
    {
        _dbContext = dbContext;
        _accountGroupService = accountGroupService;
    }

    protected override async Task Handle(RemoveAccountFromGroupCommand request, CancellationToken cancellationToken)
    {
        var accountGroup = await _dbContext.AccountGroups
            .Include(x => x.Accounts) //can't include filter because need to check accounts count
            .FirstOrDefaultAsync(x => x.Id == request.AccountGroupId, cancellationToken);

        _accountGroupService.RemoveAccountFromGroup(accountGroup!, request.AccountId);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}