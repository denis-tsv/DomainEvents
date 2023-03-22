﻿using DomainEvents.Infrastructure.Interfaces;
using DomainEvents.UseCases.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DomainEvents.UseCases.Products.Commands;

public record DeleteProductCommand(int ProductId) : IRequest, IProductRequest, ITransactionRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IDbContext _dbContext;
    private readonly CategoryService _categoryService;

    public DeleteProductCommandHandler(IDbContext dbContext, CategoryService categoryService)
    {
        _dbContext = dbContext;
        _categoryService = categoryService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync(new object?[] { request.ProductId }, cancellationToken);
        
        product!.Delete();

        var categories = await _dbContext.Categories
            .Where(x => x.Products.Any(a => a.ProductId == request.ProductId))
            .Include(x => x.Products)
            .ToListAsync(cancellationToken);

        foreach (var category in categories)
        {
            _categoryService.RemoveProductFromCategory(category, request.ProductId);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}