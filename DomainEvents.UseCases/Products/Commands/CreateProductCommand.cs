using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;

namespace DomainEvents.UseCases.Products.Commands;

public record CreateProductCommand : IRequest<Product>, ITransactionRequest;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public CreateProductCommandHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product();
        
        _dbContext.Products.Add(product);

        await _sender.Send(new UpdateProductCommand(1), cancellationToken);

        return product;
    }
}