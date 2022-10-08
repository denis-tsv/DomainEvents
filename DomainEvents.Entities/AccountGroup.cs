namespace DomainEvents.Entities;

public class AccountGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<AccountGroupAccount> Accounts { get; set; } = null!;
}