using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest, ITransactionRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;

    public DeleteProductCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);
        
        product!.Delete();
    }
}