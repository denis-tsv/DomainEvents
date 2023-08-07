using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.NotificationHandlers;

public class ProductDeletedNotificationHandler : INotificationHandler<ProductDeletedNotification>
{
    private readonly IDbSets _dbSets;
    private readonly CategoryService _categoryService;

    public ProductDeletedNotificationHandler(IDbSets dbSets, CategoryService categoryService)
    {
        _dbSets = dbSets;
        _categoryService = categoryService;
    }

    public async Task Handle(ProductDeletedNotification notification, CancellationToken cancellationToken)
    {
        var categories = await _dbSets.Categories
            .Where(x => x.Products.Any(p => p.ProductId == notification.ProductId))
            .Include(x => x.Products)
            .ToListAsync(cancellationToken);

        foreach (var category in categories)
        {
            _categoryService.RemoveProductFromCategory(category, notification.ProductId);
        }
    }
}