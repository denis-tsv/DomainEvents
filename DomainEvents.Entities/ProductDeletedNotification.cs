using MediatR;

namespace DomainEvents.Entities;

public record ProductDeletedNotification(int ProductId) : INotification;
