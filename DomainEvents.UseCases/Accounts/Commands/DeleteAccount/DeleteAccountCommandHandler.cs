using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using EFCore.BulkExtensions;
using MediatR;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : AsyncRequestHandler<DeleteAccountCommand>
{
    private readonly IDbContext _dbContext;
    private readonly IPublisher _publisher;

    public DeleteAccountCommandHandler(IDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    protected override async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Accounts
            .Where(x => x.Id == request.AccountId)
            .BatchUpdateAsync(x => new Account { IsDeleted = true }, cancellationToken: cancellationToken);

        await _publisher.Publish(new AccountDeletedNotification { AccountId = request.AccountId }, cancellationToken);
    }
}