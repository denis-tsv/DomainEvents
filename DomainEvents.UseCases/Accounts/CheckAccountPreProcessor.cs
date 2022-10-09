using DomainEvents.Infrastructure.Interfaces;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Accounts;

public class CheckAccountPreProcessor<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : IAccountRequest
{
    private readonly IDbContext _dbContext;

    public CheckAccountPreProcessor(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Accounts.AnyAsync(x => x.Id == request.AccountId && !x.IsDeleted, cancellationToken))
            throw new Exception("Account doesn't exists");
    }
}