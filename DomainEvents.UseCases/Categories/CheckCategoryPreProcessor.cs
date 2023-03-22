using DomainEvents.Infrastructure.Interfaces;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Categories;

public class CheckCategoryPreProcessor<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : ICategoryRequest
{
    private readonly IDbContext _dbContext;

    public CheckCategoryPreProcessor(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken))
            throw new Exception("Category doesn't exists");
    }
}