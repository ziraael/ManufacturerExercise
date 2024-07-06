
public record OrderCreatedEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}