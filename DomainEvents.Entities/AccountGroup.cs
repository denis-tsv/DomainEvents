namespace DomainEvents.Entities;

public class AccountGroup
{
    public int Id { get; set; }

    public List<AccountGroupAccount> Accounts { get; set; } = null!;
}