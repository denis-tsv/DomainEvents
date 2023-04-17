using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases;

public class PipelineHelper
{
    public bool Wrapped { get; set; }
}

public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ITransactionRequest
{
    private readonly IDbContext _dbContext;
    private readonly PipelineHelper _pipelineHelper;

    public TransactionPipelineBehavior(IDbContext dbContext, PipelineHelper pipelineHelper)
    {
        _dbContext = dbContext;
        _pipelineHelper = pipelineHelper;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_pipelineHelper.Wrapped) return await next();

        _pipelineHelper.Wrapped = true;

        var result = await next();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }
}