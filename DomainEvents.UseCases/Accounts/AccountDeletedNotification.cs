using MediatR;

namespace DomainEvents.UseCases.Accounts;

public class AccountDeletedNotification : INotification
{
    public int AccountId { get; set; }
}