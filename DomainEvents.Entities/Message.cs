namespace DomainEvents.Entities;

public class Message
{
    public int Id { get; set; }
    public bool IsSend { get; set; }
    public DateTime AvailableAfter { get; set; }
}