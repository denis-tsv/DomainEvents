using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories.Commands;

public record RemoveProductFromCategoryCommand(int ProductId, int CategoryId) : IRequest, ICategoryRequest, ITransactionRequest;

public class RemoveProductFromCategoryCommandHandler : IRequestHandler<RemoveProductFromCategoryCommand>
{
    private readonly IDbContext _dbContext;

    public RemoveProductFromCategoryCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RemoveProductFromCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .Include(x => x.Products) //can't include filter because need to check products count
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

        category!.RemoveProduct(request.ProductId);

        if (!category.Products.Any())
        {
            _dbContext.Categories.Remove(category);
        }
    }
}