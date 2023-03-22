using Microsoft.AspNetCore.Mvc;
using MediatR;
using DomainEvents.UseCases.Categories.Commands;

namespace DomainEvents.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpDelete("{id}/product/{productId}")]
    public async Task RemoveProduct([FromRoute] int id, [FromRoute] int productId, CancellationToken cancellationToken)
    {
        await _sender.Send(new RemoveProductFromCategoryCommand(productId, id), cancellationToken);
    }
}