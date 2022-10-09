using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.AccountGroups;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : AsyncRequestHandler<DeleteAccountCommand>
{
    private readonly IDbContext _dbContext;
    private readonly AccountGroupService _accountGroupService;

    public DeleteAccountCommandHandler(IDbContext dbContext, AccountGroupService accountGroupService)
    {
        _dbContext = dbContext;
        _accountGroupService = accountGroupService;
    }

    protected override async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts.FindAsync(new object?[] { request.AccountId }, cancellationToken);

        account!.Delete();

        var accountGroups = await _dbContext.AccountGroups
            .Where(x => x.Accounts.Any(a => a.AccountId == request.AccountId))
            .Include(x => x.Accounts)
            .ToListAsync(cancellationToken);

        foreach (var accountGroup in accountGroups)
        {
            _accountGroupService.RemoveAccountFromGroup(accountGroup, request.AccountId);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}