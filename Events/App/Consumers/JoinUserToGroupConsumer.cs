using CBVSignalR.Application.Base.Event;
using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Events.App.Models;
using CBVSignalR.Events.Interfaces;

namespace CBVSignalR.Events.App.Consumers
{
    public class JoinUserToGroupConsumer : BaseConsumer<JoinUserToGroupEvent>
    {
        protected override string QueueName => "join-user-to-groupsubscription-queue";
        protected override string RoutingKey => "joinusertogroup";

        public JoinUserToGroupConsumer(
        IRabbitMqConnection connection,
        IServiceScopeFactory scopeFactory)
        : base(connection, scopeFactory)
        {
        }

        protected override async Task HandleAsync(
        JoinUserToGroupEvent @event,
        IServiceProvider serviceProvider)
        {
            var userGroupSubscriptionService =
                serviceProvider.GetRequiredService<IUserGroupSubscriptionService>();
            var groupSubscriptionService =
                serviceProvider.GetRequiredService<IGroupSubscriptionService>();
            //Kiểm tra có tồn tại group với groupName không
            var isGroupExis = await groupSubscriptionService.GetGroupSubscriptionByName(@event.GroupName);
            if (isGroupExis == null) return;
            //Kiểm tra đã tồn tại liên kết user với group đó chưa trước khi thêm mới
            bool isUserGroupExis = await userGroupSubscriptionService.IsUserGroupExis(@event.UserId, isGroupExis.Id);
            if (isUserGroupExis == true) return;
            //Thực hiện thêm mới User group
            var userGroupSubscription = new UserGroupSubscription
            {
                GroupId = isGroupExis.Id,
                UserId = @event.UserId,
                IsActive = true
            };
            await userGroupSubscriptionService.CreateAsync(userGroupSubscription);
        }
    }
}
