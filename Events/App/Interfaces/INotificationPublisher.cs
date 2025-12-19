namespace CBVSignalR.Events.App.Interfaces
{
    public interface INotificationPublisher
    {
        Task SendToUser(string userId, object payload);
    }
}
