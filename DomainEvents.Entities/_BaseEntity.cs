using MediatR;

namespace DomainEvents.Entities;

public abstract class BaseEntity
{
    protected List<INotification> Notifications = new();

    public List<INotification> FetchNotifications()
    {
        var result = Notifications.ToList();

        Notifications.Clear();

        return result;
    }
}