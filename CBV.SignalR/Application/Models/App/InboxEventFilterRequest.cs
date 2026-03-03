namespace CBVSignalR.Application.Models.App
{
    public class InboxEventFilterRequest : PagingFilterRequest
    {
        public string? Type { get; set; } = null!;
        public string? Payload { get; set; } = null!;
        public string? Status { get; set; } = null!;
        public DateTime? ReceivedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
