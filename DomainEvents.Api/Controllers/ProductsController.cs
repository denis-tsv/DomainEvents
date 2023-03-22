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

    [HttpDelete("{id}")]
    public async Task Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteProductCommand(id), cancellationToken);
    }
}