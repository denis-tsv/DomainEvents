using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.Commands;

public record RemoveProductFromCategoryCommand(int ProductId, int CategoryId) : IRequest;

public class RemoveProductFromCategoryCommandHandler : IRequestHandler<RemoveProductFromCategoryCommand>
{
    private readonly IDbContext _dbContext;

    public RemoveProductFromCategoryCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RemoveProductFromCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        await _dbContext.ProductCategories
            .Where(x => x.ProductId == request.ProductId && x.CategoryId == request.CategoryId)
            .ExecuteDeleteAsync(cancellationToken);

        await _dbContext.Categories
            .Where(x => x.Id == request.CategoryId && !x.Products.Any())
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}