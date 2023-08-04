using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;

namespace DomainEvents.UseCases.Categories;

public class CategoryService
{
    private readonly IDbSets _dbSets;

    public CategoryService(IDbSets dbSets)
    {
        _dbSets = dbSets;
    }

    public void RemoveProductFromCategory(Category category, int productId)
    {
        category.RemoveProduct(productId);

        if (!category.Products.Any())
        {
            _dbSets.Categories.Remove(category);
        }
    }
}