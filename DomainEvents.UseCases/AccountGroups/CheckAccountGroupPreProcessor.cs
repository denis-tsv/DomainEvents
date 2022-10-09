using DomainEvents.Infrastructure.Interfaces;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.AccountGroups;

public class CheckAccountGroupPreProcessor<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : IAccountGroupRequest
{
    private readonly IDbContext _dbContext;

    public CheckAccountGroupPreProcessor(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.AccountGroups.AnyAsync(x => x.Id == request.AccountGroupId, cancellationToken))
            throw new Exception("Account group doesn't exists");
    }
}