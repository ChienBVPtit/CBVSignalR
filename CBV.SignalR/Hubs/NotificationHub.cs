using CBVSignalR.Application.Const;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CBVSignalR.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IUserGroupSubscriptionService _userGroupSubscriptionService;

        public NotificationHub(IUserGroupSubscriptionService userGroupSubscriptionService)
        {
            _userGroupSubscriptionService = userGroupSubscriptionService;
        }

        //hàm connect từ user tới hub 
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            #region truy vấn db lấy tất cả các group của user để gán lại
            var lstUserGroup = _userGroupSubscriptionService.GetUserGroupSubscriptionByUserIdAsync(userId ?? "").Result;
            foreach (var item in lstUserGroup)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, item.GroupSubscription.Name);
            }
            #endregion
            await base.OnConnectedAsync();
        }

        //hàm disconnect 
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

        //Gửi notification tới tất cả user 
        public async Task SendNotificationToAllUser(Notification notification)
        {
            await Clients.All.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        //Gửi notification tới chính user
        public async Task SendNotificationToUser(Notification notification)
        {
            await Clients.Caller.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        //Gửi notification tới các user còn lại
        public async Task SendNotificationToOrtherUser(Notification notification)
        {
            await Clients.Others.SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        //Gửi notification tới chi tiết user
        public async Task SendNotificationToUserDetail(string userId, Notification notification)
        {
            await Clients.User(userId).SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        //Gửi notification tới Group
        public async Task SendNotificationToGroup(string groupName, Notification notification)
        {
            await Clients.Group(groupName).SendAsync(
                SignalREvents.ReceiveNotification,
                notification
            );
        }

        //Đánh dấu đọc tất cả thông báo
        public async Task ReadAll()
        {
            // 1. Lấy thông tin User (ví dụ qua Context.UserIdentifier)
            var userId = Context.UserIdentifier;

            // 2. Thực hiện logic nghiệp vụ trong Database
            // _repository.MarkAllAsRead(userId);

            // 3. (Tùy chọn) Gửi phản hồi lại cho Client đã gọi hoặc các thiết bị khác của user đó
            await Clients.Caller.SendAsync("AllMessagesRead", "Tất cả thông báo đã được đánh dấu là đã đọc.");
        }
    }
}
