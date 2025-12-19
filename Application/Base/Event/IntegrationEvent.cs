namespace CBVSignalR.Application.Base.Event
{
    public abstract class IntegrationEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
