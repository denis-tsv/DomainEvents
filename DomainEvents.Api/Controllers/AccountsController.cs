using DomainEvents.UseCases.Accounts.Commands.DeleteAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DomainEvents.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpDelete("{id}")]
    public async Task DeleteAccount([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteAccountCommand { AccountId = id }, cancellationToken);
    }
}