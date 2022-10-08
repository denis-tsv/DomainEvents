using MediatR;

namespace DomainEvents.Entities;

public class AccountDeletedEvent : INotification
{
    public int AccountId { get; set; }
}