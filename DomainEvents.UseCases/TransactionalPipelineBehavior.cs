using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases;

public class TransactionalPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>, ITransactionalCommand
{
    private readonly IDbContext _dbContext;

    public TransactionalPipelineBehavior(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.GetTransactionAsync(cancellationToken);

        var result = await next();

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}