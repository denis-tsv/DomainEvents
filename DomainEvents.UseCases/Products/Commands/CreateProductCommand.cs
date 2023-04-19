using DomainEvents.Entities;
using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using MediatR.Pipeline;

namespace DomainEvents.UseCases.Products.Commands;

public record CreateProductCommand : IRequest<CreateProductCommandResult>, ITransactionRequest
{
    public Product Product { get; set; } = default!;
}

public class CreateProductCommandResult
{
    public int ProductId { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResult>
{
    private readonly IDbContext _dbContext;
    private readonly ISender _sender;

    public CreateProductCommandHandler(IDbContext dbContext, ISender sender)
    {
        _dbContext = dbContext;
        _sender = sender;
    }

    public async Task<CreateProductCommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product();
        
        _dbContext.Products.Add(product);

        await _sender.Send(new UpdateProductCommand(1), cancellationToken);

        request.Product = product;

        return new CreateProductCommandResult();
    }
}

public class CreateProductCommandPostProcessor : IRequestPostProcessor<CreateProductCommand, CreateProductCommandResult>
{
    public async Task Process(CreateProductCommand request, CreateProductCommandResult response, CancellationToken cancellationToken)
    {
        response.ProductId = request.Product.Id;
    }
}

