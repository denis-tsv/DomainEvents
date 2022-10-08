using Microsoft.AspNetCore.Mvc;
using MediatR;
using DomainEvents.UseCases.AccountGroups.Commands.RemoveAccountFromGroup;

namespace DomainEvents.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountGroupsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountGroupsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{id}")]
    public async Task RemoveAccount([FromRoute] int id, [FromBody] int accountId, CancellationToken cancellationToken)
    {
        await _sender.Send(new RemoveAccountFromGroupCommand { AccountGroupId = id, AccountId = accountId }, cancellationToken);
    }
}