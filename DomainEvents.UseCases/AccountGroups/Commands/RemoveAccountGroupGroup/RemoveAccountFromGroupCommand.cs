using MediatR;

namespace DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountGroupGroup;

public class RemoveAccountFromGroupCommand : IRequest
{
    public int AccountGroupId { get; set; }
    public int AccountId { get; set; }
}