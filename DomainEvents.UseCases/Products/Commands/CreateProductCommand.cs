using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases.Products.Commands;

public record CreateProductCommand : IRequest<int>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IDbContext _dbContext;

    public CreateProductCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product();
        
        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}

