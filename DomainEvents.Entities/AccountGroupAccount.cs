namespace DomainEvents.Entities;

public class AccountGroupAccount
{
    public int AccountId { get; set; }
    public int AccountGroupId { get; set; }

    public Account Account { get; set; } = null!;
    public AccountGroup AccountGroup { get; set; } = null!;
}