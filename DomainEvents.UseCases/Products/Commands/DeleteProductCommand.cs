using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.Categories.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public DeleteProductCommandHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        await _dbContext.Products
            .Where(x => x.Id == request.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true), cancellationToken);

        var categoryIds = await _dbContext.ProductCategories
            .Where(x => x.ProductId == request.ProductId)
            .Select(x => x.CategoryId)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var categoryId in categoryIds)
        {
            await _sender.Send(new RemoveProductFromCategoryCommand(request.ProductId, categoryId), cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }
}