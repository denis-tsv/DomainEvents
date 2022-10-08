using MediatR;

namespace DomainEvents.Entities;

public abstract class BaseEntity
{
    protected List<INotification> Events = new();

    public List<INotification> FetchEvents()
    {
        var result = Events.ToList();

        Events.Clear();

        return result;
    }
}