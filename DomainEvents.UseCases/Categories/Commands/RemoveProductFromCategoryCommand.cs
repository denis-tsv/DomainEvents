using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.Commands;

public record RemoveProductFromCategoryCommand(int ProductId, int CategoryId) : IRequest;

public class RemoveProductFromCategoryCommandHandler : IRequestHandler<RemoveProductFromCategoryCommand>
{
    private readonly IDbContext _dbContext;
    private readonly IMessageBroker _messageBroker;

    public RemoveProductFromCategoryCommandHandler(IDbContext dbContext, IMessageBroker messageBroker)
    {
        _dbContext = dbContext;
        _messageBroker = messageBroker;
    }

    public async Task Handle(RemoveProductFromCategoryCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        var category = await _dbContext.Categories
            .Include(x => x.Products) //can't include filter because need to check products count
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        category!.RemoveProduct(request.ProductId);

        if (!category.Products.Any())
        {
            _dbContext.Categories.Remove(category);
        }

        var message = new Message { AvailableAfter = DateTime.UtcNow.AddSeconds(30) };
        _dbContext.Messages.Add(message);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken); //doing nothing

        await _messageBroker.SendMessageAsync(message, cancellationToken); //fail will rollback transaction

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync(cancellationToken); //fail will rollback transaction, but message will stay in message broker
    }
}