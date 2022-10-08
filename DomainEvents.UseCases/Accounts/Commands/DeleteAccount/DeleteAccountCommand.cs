using MediatR;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommand : IRequest, ITransactionalCommand
{
    public int AccountId { get; set; }
}