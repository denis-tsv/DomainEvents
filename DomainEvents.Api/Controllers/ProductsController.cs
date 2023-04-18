using DomainEvents.UseCases.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DomainEvents.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<int> Create(CancellationToken cancellationToken)
    {
        return await _sender.Send(new CreateProductCommand(), cancellationToken);
    }

    [HttpPut("{id:int}")]
    public Task Update([FromRoute] int id, CancellationToken cancellationToken)
    {
        return _sender.Send(new UpdateProductCommand(id), cancellationToken);
    }

    [HttpDelete("{id:int}")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteProductCommand(id), cancellationToken);
    }
}