using CBVSignalR.Application.Const;
using CBVSignalR.Application.Entities;
using CBVSignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CBVSignalR.Application.Services
{
    public class EventService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public EventService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendNotificationToUser(string userId, Notification notification)
        {
            await _hub.Clients.User(userId)
                .SendAsync(SignalREvents.ReceiveNotification, notification);
        }
    }
}
