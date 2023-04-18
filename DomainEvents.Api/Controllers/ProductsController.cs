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

    [HttpDelete("{id:int}")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteProductCommand(id), cancellationToken);
    }

    [HttpPost]
    public async Task<int> Create(CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new CreateProductCommand(), cancellationToken);
        return response.ProductId;
    }

    [HttpPut("{id:int}")]
    public Task Update([FromRoute]int id, CancellationToken cancellationToken)
    {
        return _sender.Send(new UpdateProductCommand(id), cancellationToken);
    }
}