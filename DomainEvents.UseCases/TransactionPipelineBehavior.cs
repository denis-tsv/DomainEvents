using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases;

public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ITransactionRequest
{
    private readonly IDbContext _dbContext;

    public TransactionPipelineBehavior(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        var result = await next();

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}