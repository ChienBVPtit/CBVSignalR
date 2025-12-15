using CBVSignalR.Events.Interfaces;
using CBVSignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CBVSignalR.Events.Publishers
{
    public class SignalRNotificationPublisher : INotificationPublisher
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationPublisher(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToUser(string userId, object payload)
        {
            await _hub.Clients.User(userId)
                .SendAsync("notification", payload);
        }
    }
}
