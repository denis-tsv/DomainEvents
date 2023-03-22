using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.Categories.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.NotificationHandlers;

public class ProductDeletedNotificationHandler : INotificationHandler<ProductDeletedNotification>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public ProductDeletedNotificationHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task Handle(ProductDeletedNotification notification, CancellationToken cancellationToken)
    {
        var categoryIds = await _dbContext.ProductCategories
            .Where(x => x.ProductId == notification.ProductId)
            .Select(x => x.CategoryId)
            .Distinct()
            .ToListAsync(cancellationToken);

        // Command handlers shares not thread safe DbContext, so we send command sequently, but not in parallel
        foreach (var categoryId in categoryIds)
        {
            await _sender.Send(new RemoveProductFromCategoryCommand(notification.ProductId, categoryId), cancellationToken);
        }
    }
}