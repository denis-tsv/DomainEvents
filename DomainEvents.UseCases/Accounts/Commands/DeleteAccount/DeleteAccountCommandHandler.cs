using DomainEvents.Infrastructure.Interfaces;
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
        var account = await _dbContext.Accounts.FindAsync(new object[] { request.AccountId }, cancellationToken);
        if (account == null) throw new InvalidOperationException("Account not found");

        account.Delete();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}