using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountGroupGroup;

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
            .Include(x => x.Accounts)
            .SingleOrDefaultAsync(x => x.Id == request.AccountGroupId, cancellationToken);
        
        if (accountGroup == null) throw new InvalidOperationException("AccountGroup not found");

        accountGroup.Accounts.RemoveAll(x => x.AccountId == request.AccountId);

        if (!accountGroup.Accounts.Any())
        {
            _dbContext.AccountGroups.Remove(accountGroup);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }   
    
}