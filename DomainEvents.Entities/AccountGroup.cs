namespace DomainEvents.Entities;

public class AccountGroup : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<AccountGroupAccount> Accounts { get; set; } = null!;

    public void RemoveAccount(int accountId)
    {
        Accounts.RemoveAll(x => x.AccountId == accountId);
    }
}