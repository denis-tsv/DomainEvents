using MediatR;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommand : IRequest, IAccountRequest, ITransactionRequest
{
    public int AccountId { get; set; }
}