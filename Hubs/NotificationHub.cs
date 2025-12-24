using CBVSignalR.Application.Const;
using CBVSignalR.Application.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CBVSignalR.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            foreach (var claim in Context.User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
            Console.WriteLine($"UserId: {userId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            await Clients.All.SendAsync(
                "UserDisconnected",
                userId
            );

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task SendNotificationToAllUser(Notification notification)
        {
            await Clients.All.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        public async Task SendNotificationToUser(Notification notification)
        {
            await Clients.Caller.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        public async Task SendNotificationToOrtherUser(Notification notification)
        {
            await Clients.Others.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        public async Task SendNotificationToUserDetail(string userId, Notification notification)
        {
            await Clients.User(userId).SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        public async Task SendNotificationToGroup(string groupName, Notification notification)
        {
            await Clients.Group(groupName).SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }
    }
}
