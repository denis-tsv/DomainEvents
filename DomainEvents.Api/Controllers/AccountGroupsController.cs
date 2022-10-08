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

    [HttpDelete("{id}/account/{accountId}")]
    public async Task RemoveAccount([FromRoute] int id, [FromRoute] int accountId, CancellationToken cancellationToken)
    {
        await _sender.Send(new RemoveAccountFromGroupCommand { AccountGroupId = id, AccountId = accountId }, cancellationToken);
    }
}