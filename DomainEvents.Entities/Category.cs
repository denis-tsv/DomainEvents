namespace DomainEvents.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<ProductCategory> Products { get; set; } = null!;
}