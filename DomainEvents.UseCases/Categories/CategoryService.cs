using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;

namespace DomainEvents.UseCases.Categories;

public class CategoryService
{
    private readonly IDbContext _dbContext;

    public CategoryService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void RemoveProductFromCategory(Category category, int productId)
    {
        category.RemoveProduct(productId);

        if (!category.Products.Any())
        {
            _dbContext.Categories.Remove(category);
        }
    }
}