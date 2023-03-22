using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest, IProductRequest, ITransactionRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;
    private readonly IPublisher _publisher;

    public DeleteProductCommandHandler(IDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(x => x.Id == request.ProductId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true), cancellationToken);

        await _publisher.Publish(new ProductDeletedNotification(request.ProductId), cancellationToken);
    }
}