namespace DomainEvents.Entities;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<ProductCategory> Products { get; set; } = null!;

    public void RemoveProduct(int productId)
    {
        Products.RemoveAll(x => x.ProductId == productId);
    }
}