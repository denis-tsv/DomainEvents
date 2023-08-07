using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;
    private readonly IExternalService _externalService;

    public DeleteProductCommandHandler(IDbContext dbContext, IExternalService externalService)
    {
        _dbContext = dbContext;
        _externalService = externalService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        var product = await _dbContext.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);
        
        product!.Delete();

        await _dbContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        await _externalService.LongOperationAsync(cancellationToken);
    }
}