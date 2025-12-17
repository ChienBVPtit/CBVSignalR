using CBVSignalR.Application.Base.Entity;

namespace CBVSignalR.Application.Entities
{
    public class InboxEvent : BaseEntity
    {
        public string Type { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
    }
}
