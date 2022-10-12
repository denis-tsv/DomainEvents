using MediatR;

namespace DomainEvents.Entities;

public class AccountDeletedNotification : INotification
{
    public int AccountId { get; set; }
}