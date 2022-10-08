namespace DomainEvents.Entities;

public class Account : BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;

        Events.Add(new AccountDeletedEvent { AccountId = Id });
    }
}