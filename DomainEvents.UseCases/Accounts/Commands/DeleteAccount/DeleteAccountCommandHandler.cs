using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var account = await _dbContext.Accounts.FindAsync(new object[] { request.AccountId }, cancellationToken);
        if (account == null) throw new InvalidOperationException("Account not found");

        account.IsDeleted = true;

        var accountGroups = await _dbContext.AccountGroups
            .Where(x => x.Accounts.Any(a => a.AccountId == request.AccountId))
            .Include(x => x.Accounts)
            .ToListAsync(cancellationToken);

        foreach (var accountGroup in accountGroups)
        {
            accountGroup.Accounts.RemoveAll(x => x.AccountId == request.AccountId);

            if (!accountGroup.Accounts.Any())
            {
                _dbContext.AccountGroups.Remove(accountGroup);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}