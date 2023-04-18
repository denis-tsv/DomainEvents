using DomainEvents.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products.Commands;

public record UpdateProductCommand(int Id) : IRequest;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IDbContext _dbContext;

    public UpdateProductCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        await _dbContext.Products
            .Where(x => x.Id == request.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsDeleted, false), cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}