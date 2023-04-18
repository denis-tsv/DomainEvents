using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;

    public DeleteProductCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        await _dbContext.Products
            .Where(x => x.Id == request.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true), cancellationToken);

        await _dbContext.ProductCategories
            .Where(x => x.ProductId == request.ProductId)
            .ExecuteDeleteAsync(cancellationToken);

        await _dbContext.Categories
            .Where(x => !x.Products.Any())
            .ExecuteDeleteAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}