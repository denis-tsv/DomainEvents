namespace DomainEvents.Infrastructure.Interfaces;

public interface IExternalService
{
    // up to 1 minute
    Task LongOperationAsync(CancellationToken token);
}