using MediatR;

namespace DomainEvents.UseCases.Products;

public record ProductDeletedNotification(int ProductId) : INotification;
