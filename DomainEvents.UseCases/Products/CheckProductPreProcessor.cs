using DomainEvents.Infrastructure.Interfaces;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products;

public class CheckProductPreProcessor<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : IProductRequest
{
    private readonly IDbContext _dbContext;

    public CheckProductPreProcessor(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Products.AnyAsync(x => x.Id == request.ProductId && !x.IsDeleted, cancellationToken))
            throw new Exception("Product doesn't exists");
    }
}