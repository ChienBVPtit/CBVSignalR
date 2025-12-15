namespace CBVSignalR.Events.Interfaces
{
    public interface INotificationPublisher
    {
        Task SendToUser(string userId, object payload);
    }
}
