using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Context;
using CBVSignalR.Events.App.Interfaces;
using CBVSignalR.Events.App.Models;
using Newtonsoft.Json;

namespace CBVSignalR.Events.App.ConsumerHandlers
{
    public class JoinUserToGroupHandler : IJoinUserToGroupHandler
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserGroupSubscriptionService _userGroupService;
        private readonly IGroupSubscriptionService _groupService;
        private readonly IInboxEventService _inboxService;
        private readonly ILogger<JoinUserToGroupHandler> _logger;

        public JoinUserToGroupHandler(
            ApplicationDbContext db,
            IUserGroupSubscriptionService userGroupService,
            IGroupSubscriptionService groupService,
            IInboxEventService inboxService,
            ILogger<JoinUserToGroupHandler> logger)
        {
            _db = db;
            _userGroupService = userGroupService;
            _groupService = groupService;
            _inboxService = inboxService;
            _logger = logger;
        }

        public async Task HandleAsync(JoinUserToGroupEvent @event)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            // 1️⃣ Inbox – Idempotent
            var processed = await _inboxService.GetByIdAsync(@event.EventId);
            if (processed != null)
            {
                _logger.LogWarning(
                    "⚠️ Event already processed | EventId={EventId}",
                    @event.EventId);

                await tx.CommitAsync();
                return;
            }

            await _inboxService.CreateAsync(new InboxEvent
            {
                Id = @event.EventId,
                Type = nameof(JoinUserToGroupEvent),
                ReceivedAt = DateTime.UtcNow,
                Payload = JsonConvert.SerializeObject(@event)
            });

            // 2️⃣ Business
            var group = await _groupService.GetGroupSubscriptionByName(@event.GroupName);
            if (group == null)
            {
                _logger.LogWarning(
                    "⚠️ Group not found | EventId={EventId} | Group={Group}",
                    @event.EventId,
                    @event.GroupName);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return;
            }

            if (await _userGroupService.IsUserGroupExis(@event.UserId, group.Id))
            {
                _logger.LogInformation(
                    "ℹ️ User already in group | UserId={UserId} | GroupId={GroupId}",
                    @event.UserId,
                    group.Id);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return;
            }

            await _userGroupService.CreateAsync(new UserGroupSubscription
            {
                GroupId = group.Id,
                UserId = @event.UserId,
                IsActive = true
            });

            // 3️⃣ Commit
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
    }
}
