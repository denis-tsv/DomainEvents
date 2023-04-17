using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases;

public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ITransactionRequest
{
    private readonly IDbContext _dbContext;
    private bool _wrapped;

    public TransactionPipelineBehavior(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_wrapped) return await next();

        _wrapped = true;

        var result = await next();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }
}