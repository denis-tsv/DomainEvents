namespace DomainEvents.Entities;

public class Account : BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;

        Notifications.Add(new AccountDeletedNotification { AccountId = Id });
    }
}