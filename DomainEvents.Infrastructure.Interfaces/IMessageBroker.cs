namespace DomainEvents.Infrastructure.Interfaces;

public interface IMessageBroker
{
    Task SendMessageAsync(object message, CancellationToken token);
}