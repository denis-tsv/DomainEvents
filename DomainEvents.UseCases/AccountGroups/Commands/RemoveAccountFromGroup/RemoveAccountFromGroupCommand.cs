using MediatR;

namespace DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;

public class RemoveAccountFromGroupCommand : IRequest, IAccountGroupRequest, ITransactionRequest
{
    public int AccountGroupId { get; set; }
    public int AccountId { get; set; }
}