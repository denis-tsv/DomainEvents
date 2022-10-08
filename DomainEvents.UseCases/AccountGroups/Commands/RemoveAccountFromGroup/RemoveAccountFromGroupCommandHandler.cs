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
            .Include(x => x.Accounts)
            .SingleOrDefaultAsync(x => x.Id == request.AccountGroupId, cancellationToken);
        
        if (accountGroup == null) throw new InvalidOperationException("AccountGroup not found");

        _accountGroupService.RemoveAccountFromAccountGroup(accountGroup, request.AccountId, _dbContext);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }   
    
}