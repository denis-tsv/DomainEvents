using MediatR;

namespace DomainEvents.UseCases.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommand : IRequest
{
    public int AccountId { get; set; }
}