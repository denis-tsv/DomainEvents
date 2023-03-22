namespace DomainEvents.Entities;

public class Product : BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public void Delete()
    {
        IsDeleted = true;
    }
}