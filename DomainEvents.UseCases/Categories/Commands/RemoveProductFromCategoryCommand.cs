using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.Commands;

public record RemoveProductFromCategoryCommand(int ProductId, int CategoryId) : IRequest;

public class RemoveProductFromCategoryCommandHandler : IRequestHandler<RemoveProductFromCategoryCommand>
{
    private readonly IDbContext _dbContext;
    private readonly CategoryService _categoryService;
    private readonly IMessageBroker _messageBroker;

    public RemoveProductFromCategoryCommandHandler(IDbContext dbContext, CategoryService categoryService, IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _categoryService = categoryService;
        _messageBroker = messageBroker;
    }

    public async Task Handle(RemoveProductFromCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        var category = await _dbContext.Categories
            .Include(x => x.Products) //can't include filter because need to check products count
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        _categoryService.RemoveProductFromCategory(category!, request.ProductId);

        var message = new Message { AvailableAfter = DateTime.UtcNow.AddSeconds(30) };
        _dbContext.Messages.Add(message);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await _messageBroker.SendMessageAsync(message, cancellationToken);

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}